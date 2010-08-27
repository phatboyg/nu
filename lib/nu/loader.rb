require 'rubygems'
require 'rubygems/dependency_installer'
require File.expand_path(File.dirname(__FILE__) + "/lib_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/gem_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/project.rb")

module Nu
  class Loader
    
		attr :gem_name
		attr :location
		attr :version
		
    def initialize(name, location, version)
      #improve the crap out of this
      @gem_name = name
      @location = location
      @version = version
    end

		def load_gem
			if !gem_available?
        puts "Gem #{(@gem_name + " #{@version}").strip} is not installed locally - I am now going to try and install it"
        begin
          inst = Gem::DependencyInstaller.new
          inst.install @gem_name, @version
          inst.installed_gems.each do |spec|
            puts "Successfully installed #{spec.full_name}"
          end
        rescue Gem::GemNotFoundException => e
          puts "ERROR: #{e.message}"
          return false
        end
      else
        return true
      end
		end

		def copy_to_lib
			start_here = copy_source
			puts "Copy From: #{start_here}"

			to = copy_dest
			puts "Copy To: #{to}"

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
      proj = Nu::Project.new

			Nu::LibTools.folder_for(gemspec, @location, proj.should_use_long_names?)
    end

    def process_dependencies
      gemspec.dependencies.each do |d|
        if Gem.available? d.name
          puts "Loading dependency: #{d.name} #{d.requirement}"
					loader = Loader.new(d.name, @location, d.requirement)
					loader.copy_to_lib
        else
          puts "#{d.name} is not installed locally"
          puts "please run 'gem install #{d.name}"
        end
      end
    end

  end
end
