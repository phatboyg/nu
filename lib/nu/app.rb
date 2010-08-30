#!/usr/bin/env ruby 

require 'optparse' 
require 'ostruct'
require 'date'
require File.expand_path(File.dirname(__FILE__) + "/api.rb")
require File.expand_path(File.dirname(__FILE__) + "/loader.rb")

class App
  
  attr_reader :options

  def initialize(arguments, stdin, stdout)
    
		#special case, they want to know our version
		if arguments.length == 1 && arguments[0] == '--version'
			output_version
			exit 0
		end
		
		Nu::Api.out(stdout)

		@arguments = arguments
    
    # Set defaults
    @options = OpenStruct.new
    @options.verbose = false
    @options.quiet = false
		@commands = []

	begin
		OptionParser.new do |opts|

			opts.banner = ["Usage: nu install PACKAGE [options]",
										"\nConfig: nu config NAME VALUE",
										"\nReport: nu report"]
		
			opts.on('-v', '--version VERSION','Specify version of package to install' ) do |ver|
				 @options.package_version = ver
			end
		
			opts.on('-r','--report', 'Report on the packages currently installed in the lib folder') do
				@commands << lambda {Nu::Api.output_report}
			end

			# Specify options
			opts.on('-V', '--verbose')    { @options.verbose = true }  
			opts.on('-q', '--quiet')      { @options.quiet = true }

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
      
      puts "Start at #{DateTime.now}\n\n" if @options.verbose
      output_version if @options.verbose
      output_inputs if @options.verbose
      
			Nu::Api.verbose(@options.verbose)
			Nu::Api.load_project_settings('nuniverse.yaml')
			
			@commands.reverse.each {|command| command.call}
      
      puts "\nFinished at #{DateTime.now}" if @options.verbose
      
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
					@commands << lambda {Nu::Api.output_report}
				when 'install'
					assert_param_count(2)
					@options.package = @arguments[1]
					@commands << lambda {Nu::Api.install_package(@options.package, @options.package_version)}
				# when 'uninstall'
				# 				assert_param_count(2)
				# 				@options.package = @arguments[1]
				# 				@commands << lambda {Nu::Api.uninstall_package(@options.package, @options.package_version)}
				when 'config'
					if @arguments.length == 2
						@commands << lambda {puts "#{@arguments[1]} = #{Nu::Api.get_setting(@arguments[1])}"} 
					else
						assert_param_count(3)
						@commands << lambda do
							Nu::Api.store_setting(@arguments[1], @arguments[2])
							puts "#{@arguments[1]} = #{Nu::Api.get_setting(@arguments[1])}"
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
      puts "Inputs:\n"
      
      @options.marshal_dump.each do |name, val|        
        puts "  #{name} = #{val}"
      end
			puts ""
    end
    
		def output_help(opts)
			output_version
			puts opts
      exit 0
		end

    def output_version
			puts Nu::Api.version_string
    end

end