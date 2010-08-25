require 'rubygems'
require 'rubygems/dependency_installer'

module Nu
  class Loader
    
    def self.load(name)
      load(name, 'lib')
    end

    def self.load(name, location, version)
      #improve the crap out of this
      @gem_to_copy = name
      @location = location
      
      if !Gem.available? @gem_to_copy
        puts "Gem #{@gem_to_copy} is not installed locally - I am now going to try and install it"
        begin
          inst = Gem::DependencyInstaller.new
          inst.install @gem_to_copy, version
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

      start_here = get_copy_from()
      puts "Copy From: #{start_here}"

      to = get_copy_to()
      puts "Copy To: #{to}"

      FileUtils.copy_entry start_here, to
	  
      process_dependencies
    end

    def self.get_libdir(name)
      gem = get_gemspec name
      #puts "GemSpec #{gem.full_gem_path}"
      gem_path = gem.full_gem_path
      libdir = File.join(gem_path,"lib")
      unless File.exist?(libdir)
        puts "Getting libdir from #{File.join(gem_path, '.require_paths')}"
        libdir = IO.readlines(File.join(gem_path, ".require_paths"))[0].strip 
        libdir = File.expand_path(File.join(gem_path,libdir))
      end
      libdir
    end

    def self.get_gemspec(name)
      gems = Gem.source_index.find_name name
      return gems.last if gems.length > 0
    end

    def self.get_copy_from
      libdir = get_libdir @gem_to_copy
      #puts File.expand_path libdir
      #try Dir.glob("{bin,lib}/**/*")
      libdir.gsub '{lib}','lib'
    end

    def self.get_files
      spec = get_gemspec @gem_to_copy
      files = spec.lib_files #get full path
      files
    end

    def self.get_copy_to
      proj = Nu::Project.new

      spec = get_gemspec @gem_to_copy
      #to be used in copying
      if proj.should_use_long_names?
        name = spec.full_name
      else
        name = spec.name
      end
      to = Dir.pwd + "/#{@location}/#{name}"
      if Dir[to] == [] #may need a smarter guy here
        FileUtils.mkpath to
      end
      to
    end

    def self.process_dependencies
      spec = get_gemspec @gem_to_copy
      spec.dependencies.each do |d|
        if Gem.available? d.name
          puts "loading #{d.name}"
          load d.name, @location
        else
          puts "#{d.name} is not installed locally"
          puts "please run 'gem install #{d.name}"
        end
      end
    end

  end
end
