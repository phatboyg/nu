require 'rubygems'
require 'ostruct'

require File.expand_path(File.dirname(__FILE__) + "/lib_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/gem_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/settings.rb")

module Nu
	class Api
		
		def self.set_log(logger)
			@log = logger
		end
		
		def self.set_out(outer)
			@out = outer
		end
	
		def self.load_project_settings(settings_file)
			log "Load Project Settings called: settings_file=#{settings_file}"
			@settings_file = settings_file

			@platforms = 
			['net1_0', 'net1_1', 'net2_0', 'net3_0', 'net3_5', 'net4_0',
				'mono1_0', 'mono2_0', 
				'silverlight_2_0', 'silverlight_3_0', 'silverlight_4_0']
			
			@project_settings = YAML.load_file(@settings_file) if File.exist?(@settings_file)
			@project_settings = OpenStruct.new if @project_settings == nil
			Nu::SettingsExtension.mix_in(@project_settings)
			
			#set defaults just in case they didn't load
			@project_settings.lib = OpenStruct.new if @project_settings.lib == nil
			@project_settings.lib.location = './lib' if @project_settings.lib.location == nil
			@project_settings.lib.use_long_names = false if @project_settings.lib.use_long_names == nil

			if @verbose
				log "Project Settings:"
				log YAML.dump(@project_settings).gsub('!ruby/object:OpenStruct','').gsub(/\s*table:/,'').gsub('---','')
				log ""
			end
		end
	
		def self.store_setting(name, value)
			log "Store Setting called: name=#{name} value=#{value}"
			log "Before:"
			load_project_settings(@settings_file) if @verbose
			
			@project_settings.set_setting_by_path(name, value, @log)
			assert_project_settings
			
			File.open(@settings_file, 'w') do |out|
	 			YAML.dump(@project_settings, out)
		  end
			log "After:"
			load_project_settings(@settings_file)
		end
	
		def self.assert_project_settings
			if @project_settings.platform
				@project_settings.platform = @project_settings.platform.gsub('.','_')
				if !@platforms.index(@project_settings.platform)
					out "'#{@project_settings.platform}' is not a valid platform." 
					out "\nChoose one of these:"
					@platforms.each {|p| disp "  #{p}"}
					out ""
					exit 1
				end
			end
		end
	
		def self.get_setting(name)
			log "Get Setting called: name=#{name}"
			@project_settings.get_setting_by_path(name, @log)
		end
	
		def self.version_string
			nu_spec = Nu::GemTools.spec_for("nu")
			"Nubular, version #{nu_spec.version}"
		end
		
		def self.install_package(package_name, package_version)
			log "Install called: package_name=#{package_name} package_version=#{package_version}."
			
			loader = Nu::Loader.new(package_name, package_version, @project_settings.lib.location, @project_settings.lib.use_long_names, @out, @log)
			if loader.load_gem
				 loader.copy_to_lib
			end
			
		end
		
		def self.report()
			log "Report called."
			Nu::LibTools.read_specs_from_lib(@project_settings.lib.location)
		end
		
		private 
		
			def self.log(msg)
				@log.call(msg)
			end
		
			def self.out(msg)
				@out.call(msg)
			end
			
		end
end