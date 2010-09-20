#!/usr/bin/env ruby 

require 'optparse' 
require 'ostruct'
require 'date'
require 'logger'
require File.expand_path(File.dirname(__FILE__) + "/api.rb")
require File.expand_path(File.dirname(__FILE__) + "/loader.rb")
require File.expand_path(File.dirname(__FILE__) + "/cli_shim.rb")
require File.expand_path(File.dirname(__FILE__) + "/json_shim.rb")
require File.expand_path(File.dirname(__FILE__) + "/help/help.rb")  
		
class App
  include Help

  attr_reader :options

  def initialize(arguments, stdin, stdout)

		# Set defaults
    @options = OpenStruct.new
    @options.verbose = false
    @options.quiet = false
		@commands = []    

		@arguments = arguments

		Nu::Api.set_log(lambda {|msg| log msg})
		Nu::Api.set_out(lambda {|msg| disp msg})
		@shim = CliShim.new(lambda {|msg| disp msg}, lambda {|msg| log msg})
		
		#special case, they want to know our version
		if arguments.length == 1 && (arguments[0] == '--version' || arguments[0] == '-v')
			@commands << lambda {output_version}
			execute_commands
			exit 0
		end
		
		begin
			OptionParser.new do |opts|

				opts.on('-v', '--version VERSION') do |ver|
					 @options.package_version = ver
				end

				opts.on('-r','--report') do
					@commands << lambda {@shim.report}
				end

				# Specify options
				@options.source = :lib
				opts.on('--remote') {@options.source = :remote}
				opts.on('--cache') {@options.source = :cache unless @options.source == :remote}
				opts.on('-V', '--verbose')    { @options.verbose = true }  
				opts.on('-q', '--quiet')      { @options.quiet = true }
				opts.on('--json') {set_json}

				opts.on_tail( '-h', '--help' ) do
					output_help
				end
	
				@help_command = lambda{output_help}
	
		end.parse!
	rescue
		@commands << @help_command
		execute_commands
		exit 0
	end
	
		post_process_options
		extract_commands
  end

  def run
        
    if arguments_valid? 
      
      log "Start at #{DateTime.now}\n\n"
      output_inputs if @options.verbose
      
			execute_commands

      log "\nFinished at #{DateTime.now}"
      
    else
      @commands << @help_command
			execute_commands
			exit 0
    end
      
  end
  
  protected

		def execute_commands
			Nu::Api.load_project_settings('nuniverse.yaml')
			@commands.reverse.each {|command| command.call}
		end

		def extract_commands
			if @arguments.length > 0
				@options.command = @arguments[0].downcase
				case @options.command
				when 'report'
					@commands << lambda {@shim.report}
				when 'specification'
					if @options.source == :lib && @options.package_version
						puts "The --version flag cannot be used with the specification command unless either --cache or --remote is also specified." 
						exit 1
					end
					set_json
					@commands << lambda {@shim.specification(@arguments[1], @options.package_version, @options.source)}
				when 'install'
					assert_param_count(2)
					@options.package = @arguments[1]
					@commands << lambda {@shim.install_package(@options.package, @options.package_version)}
				# when 'uninstall'
				# 				assert_param_count(2)
				# 				@options.package = @arguments[1]
				# 				@commands << lambda {@shim.uninstall_package(@options.package, @options.package_version)}
				when 'propose'
					assert_param_count(2)
					@options.package = @arguments[1]
					@commands << lambda {@shim.propose(@options.package, @options.package_version)}
				when 'help'
					@commands << lambda {output_help(@arguments)}
				when 'config'
					if @arguments.length == 2
						@commands << lambda {@shim.get_setting(@arguments[1])} 
					else
						assert_param_count(3)
						@commands << lambda do
							Nu::Api.store_setting(@arguments[1], @arguments[2])
							@shim.get_setting(@arguments[1])
						end if @arguments.length == 3
					end
				end
			end
		end

		def assert_param_count(count)
			unless @arguments.length == count
				@help_command.call
			end
		end

    def post_process_options
		  @options.verbose = false if !@options.verbose
      @options.verbose = false if @options.quiet
    end
    
    # True if required arguments were provided
    def arguments_valid?
      true if @commands.length > 0
    end

    def output_inputs
      disp "Inputs:\n"
      
      @options.marshal_dump.each do |name, val|        
        disp "  #{name} = #{val}"
      end
			disp ""
    end
    
		def output_help(descriptor='help-usage')
			@options.quiet = false
			output_version
			disp ''
			disp help_doc_for(descriptor)
			disp ''
      exit 0
		end

    def output_version
			disp Nu::Api.version_string
    end
		
		def disp(msg)
			puts msg unless @options.quiet || @options.json
		end
		
		def log(msg)
			log_to_file(msg) if @options.json
			if @options.verbose
				puts msg unless @options.json
			end
		end
		
		def log_to_file(msg)
			if @options.verbose
				@file_logger ||= Logger.new('nu.log', 5, 1024000)	
				@file_logger.debug("#{DateTime.now} - #{msg.strip}")
			end
		end
		
		def set_json
			@options.json = true
			@shim = JsonShim.new(lambda do |json|
				 puts json
				 log_to_file(json)
				end, lambda {|msg| log msg})
		end
end