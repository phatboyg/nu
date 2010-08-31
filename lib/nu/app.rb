#!/usr/bin/env ruby 

require 'optparse' 
require 'ostruct'
require 'date'
require 'logger'
require File.expand_path(File.dirname(__FILE__) + "/api.rb")
require File.expand_path(File.dirname(__FILE__) + "/loader.rb")
require File.expand_path(File.dirname(__FILE__) + "/cli_shim.rb")
require File.expand_path(File.dirname(__FILE__) + "/json_shim.rb")

class App
  
  attr_reader :options

  def initialize(arguments, stdin, stdout)

		# Set defaults
    @options = OpenStruct.new
    @options.verbose = false
    @options.quiet = false
		@commands = []    

		@arguments = arguments

		#special case, they want to know our version
		if arguments.length == 1 && (arguments[0] == '--version' || arguments[0] == '-v')
			output_version
			exit 0
		end

		Nu::Api.set_log(lambda {|msg| log msg})
		Nu::Api.set_out(lambda {|msg| disp msg})
		@shim = CliShim.new(lambda {|msg| disp msg}, lambda {|msg| log msg})
		
		begin
			OptionParser.new do |opts|

				opts.banner = "\nUsage:" + 
											"\n    nu -h/--help" +
											"\n    nu -v/--version" +
											"\n    nu COMMAND [arguments...] [options...]" +
											"\n\nExamples:" +
											"\n    nu install fluentnhibernate" + 
											"\n    nu install nunit --version 2.5.7.10213.20100801" +
											"\n    nu config lib.location ./lib" +
											"\n    nu report" +
											"\n    nu install fluentnhibernate --report" +
											"\n\nOptions and Switches:" 

				opts.on('-v', '--version VERSION','Specify version of package to install' ) do |ver|
					 @options.package_version = ver
				end

				opts.on('-r','--report', 'Report on the packages currently installed in the lib folder. When called as a switch it will run the report AFTER executing the requested command.') do
					@commands << lambda {@shim.report}
				end

				# Specify options
				opts.on('-V', '--verbose')    { @options.verbose = true }  
				opts.on('-q', '--quiet')      { @options.quiet = true }
				opts.on('--json', 'Run in JSON mode. All outputs will be in JSON, status messages silenced.') do 
					@options.json = true
					@shim = JsonShim.new(lambda do |json|
						 puts json
						 log_to_file(json)
						end, lambda {|msg| log msg})
				end

				opts.on_tail( '-h', '--help', 'Display this screen' ) do
					output_help(opts)
				end
	
				@help_command = lambda{output_help(opts)}
	
		end.parse!
	rescue
		@help_command.call
	end
	
		post_process_options
		extract_commands
  end

  def run
        
    if arguments_valid? 
      
      log "Start at #{DateTime.now}\n\n"
      output_version if @options.verbose
      output_inputs if @options.verbose
      
			Nu::Api.load_project_settings('nuniverse.yaml')
			
			@commands.reverse.each {|command| command.call}

      log "\nFinished at #{DateTime.now}"
      
    else
      @help_command.call
    end
      
  end
  
  protected

		def extract_commands
			if @arguments.length > 0
				@options.command = @arguments[0].downcase
				case @options.command
				when 'report'
					@commands << lambda {@shim.report}
				when 'install'
					assert_param_count(2)
					@options.package = @arguments[1]
					@commands << lambda {@shim.install_package(@options.package, @options.package_version)}
				# when 'uninstall'
				# 				assert_param_count(2)
				# 				@options.package = @arguments[1]
				# 				@commands << lambda {@shim.uninstall_package(@options.package, @options.package_version)}
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
    
		def output_help(opts)
			@options.quiet = false
			output_version
			output_description
			disp opts
			disp "\n\nFurther Information:" +
			"\n    http://nu.wikispot.org" +
			"\n    http://groups.google.com/group/nu-net"
			disp ''
      exit 0
		end

    def output_version
			disp Nu::Api.version_string
    end

		def output_description
			disp "Nu will automatically download and install specified library packages into your lib folder, along with all their dependencies.\n"
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
end