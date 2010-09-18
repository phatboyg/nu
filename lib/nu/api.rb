require 'rubygems'
require 'ostruct'

require File.expand_path(File.dirname(__FILE__) + "/lib_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/gem_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/settings.rb")

require File.expand_path(File.dirname(__FILE__) + "/dependency_leveling/package_conflict_overlap_resolver.rb")

class ::Hash
  def method_missing(name)
    return self[name] if key? name
    self.each { |k,v| return v if k.to_s.to_sym == name }
    super.method_missing name
  end
end

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
			
			@lib_tools = Nu::LibTools.new
			@gem_tools = Nu::GemTools.new
		end
	
		def self.store_setting(name, value)
			log "Store Setting called: name=#{name} value=#{value}"
			
			@project_settings.set_setting_by_path(name, value, @log)
			assert_project_settings
			
			File.open(@settings_file, 'w') do |out|
	 			YAML.dump(@project_settings, out)
		  end
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
			nu_spec = @gem_tools.spec_for("nu")
			"Nubular, version #{nu_spec.version}"
		end
		
		def self.install_package(package_name, package_version=nil)
			log "Install called: package_name: #{package_name} package_version: #{package_version}."
			
			current_specs = @lib_tools.read_specs_from_lib(@project_settings.lib.location)
			analyzer = PackageConflictOverlapResolver.new(current_specs, @gem_tools)
			results = analyzer.analyze_proposal(@gem_tools.remote_spec_for(package_name, package_version))
			
			unless results.conflict?
				results.suggested_packages.each do |package|
					loader = Nu::Loader.new(package.name, package.version, @project_settings.lib.location, @project_settings.lib.use_long_names, @out, @log)
					log "Installing #{package.name} (#{package.version})"
					if loader.load_gem
						 loader.copy_to_lib
					end
				end
				out "Installed package #{(package + " #{package_version}").strip}"
			else
				out "Could not install #{package_name} due to conflicts. Run `propose` to see analysis."
			end

			return results
		end
		
		def self.propose_package(package_name, package_version=nil)
			log "Propose Package called. package_name: #{package_name} package_version: #{package_version}"
			current_specs = @lib_tools.read_specs_from_lib(@project_settings.lib.location)
			
			log "Current Package Specs:"
			current_specs.each{|spec| log "#{spec.name} (#{spec.version})"}
			
			analyzer = PackageConflictOverlapResolver.new(current_specs, @gem_tools)
			analyzer.analyze_proposal(@gem_tools.remote_spec_for(package_name, package_version))
		end
		
		def self.report()
			log "Report called."
			@lib_tools.read_specs_from_lib(@project_settings.lib.location)
		end
		
		def self.retrieve_specification_with_version(name, version, source = {:from => :lib})
			log "Retrieve Specification With Version called. Name: #{name} Version: #{version} Source: #{source}"
			
			return case source[:from]
			when :cache then
				@gem_tools.spec_for(name, version)
			when :remote then
				@gem_tools.remote_spec_for(name, version)
			else
				raise "source can only be :cache, or :remote"
			end
		end
		
		def self.retrieve_specification(name, source = {:from => :lib})
			log "Retrieve Specification called. Name: #{name} Source: #{source}"
			
			return case source[:from]
			when :lib then
				installed = @lib_tools.read_specs_from_lib(@project_settings.lib.location)
				installed.select{|spec| spec.name == name}.first
			when :cache then
				@gem_tools.spec_for(name)
			when :remote then
				@gem_tools.remote_spec_for(name)
			else
				raise "source can only be :lib, :cache, or :remote"
			end
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