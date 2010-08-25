require 'rubygems'
require 'rubygems/dependency_installer'
require 'lib/nu/lib_tools'
require 'lib/nu/project'

module Nu
  class Loader
    
    def self.load(name)
      load(name, 'lib')
    end

    def self.load(name, location, version)
      #improve the crap out of this
      @gem_name = name
      @location = location
      @version = version

      if !gem_available?
        puts "Gem #{@gem_name} is not installed locally - I am now going to try and install it"
        begin
          inst = Gem::DependencyInstaller.new
          inst.install @gem_name, version
          inst.installed_gems.each do |spec|
            puts "Successfully installed #{spec.full_name}"
          end
        rescue Gem::GemNotFoundException => e
          puts "ERROR: #{e.message}"
          return #GTFO
        end
      else
        puts "Found Gem"
      end
      #TODO: better error handling flow control for above
    end

		def self.copy_to_lib
			 start_here = get_copy_from()
	      puts "Copy From: #{start_here}"

	      to = get_copy_to()
	      puts "Copy To: #{to}"

	      FileUtils.copy_entry start_here, to

	      process_dependencies
		end

		def self.gemspec
			Nu::LibTools.gemspec_for(@gem_name)
		end

		def self.gem_available?
			Gem.available? @gem_name
		end


    def self.get_libdir(name)
      #puts "GemSpec #{g.full_gem_path}"
      l = gemspec.full_gem_path
      d = File.join(l,"lib")
      #puts d
      d
    end

    def self.get_copy_from
      libdir = get_libdir @gem_name
      #puts File.expand_path libdir
      #try Dir.glob("{bin,lib}/**/*")
      libdir.gsub '{lib}','lib'
    end

    def self.get_files
      gemspec.lib_files #get full path
    end

    def self.get_copy_to
      proj = Nu::Project.new

      #to be used in copying
      if proj.should_use_long_names?
        name = gemspec.full_name
      else
        name = gemspec.name
      end
      to = Dir.pwd + "/#{@location}/#{name}"
      if Dir[to] == [] #may need a smarter guy here
        FileUtils.mkpath to
      end
      to
    end

    def self.process_dependencies
      gemspec.dependencies.each do |d|
        if Gem.available? d.name
          puts "loading #{d.name}"
          load d.name, @location, @version
        else
          puts "#{d.name} is not installed locally"
          puts "please run 'gem install #{d.name}"
        end
      end
    end

  end
end
