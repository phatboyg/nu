require 'rubygems'
require 'rubygems/dependency_installer'
require File.expand_path(File.dirname(__FILE__) + "/lib_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/gem_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")

module Nu
  class Loader < HasOutAndLog
    
		attr :gem_name
		attr :location
		attr :version
		
    def initialize(name, version, location, long_names, out, log)
      #improve the crap out of this
			@long_names = long_names
      @gem_name = name
      @location = location
      @version = version
			super(out, log)
    end

		def load_gem
			log "Load Gem #{(@gem_name + " #{@version}").strip}."
			if !gem_available?
        out "Gem #{(@gem_name + " #{@version}").strip} is not installed locally - I am now going to try and install it"
        begin
          inst = Gem::DependencyInstaller.new
          inst.install @gem_name, @version
          inst.installed_gems.each do |spec|
            out "Successfully installed #{spec.full_name}"
          end
        rescue Gem::GemNotFoundException => e
          out "ERROR: #{e.message}"
          return false
        end
      else
        return true
      end
		end

		def copy_to_lib
			start_here = copy_source
			log "Copy From: #{start_here}"

			to = copy_dest
			log "Copy To: #{to}"

			FileUtils.copy_entry start_here, to
			Nu::GemTools.write_spec(gemspec, to)

			process_dependencies
		end

		def gemspec
			Nu::GemTools.spec_for(@gem_name, @version)
		end

		def gem_available?
			if @version.nil?
			    Gem.available? @gem_name
			else
			    Gem.available? @gem_name, @version
			end
		end

    def copy_source
      Nu::GemTools.lib_for(@gem_name, @version).gsub '{lib}','lib'
    end

    def copy_dest
			Nu::LibTools.folder_for(gemspec, @location, @long_names)
    end

    def process_dependencies
      gemspec.dependencies.each do |d|
        if Gem.available? d.name
          out "Loading dependency: #{d.name} #{d.requirement}"
					loader = Loader.new(d.name, d.requirement, @location, @long_names, @out, @log)
					loader.copy_to_lib
        else
          out "#{d.name} is not installed locally"
          out "please run 'gem install #{d.name}"
        end
      end
    end

  end
end
