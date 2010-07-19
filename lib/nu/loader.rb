require 'rubygems'

module Nu
  class Loader
    
	def self.load(name)
		load(name, 'lib')
	end
	def self.load(name, location)
      #improve the crap out of this
      @gem_to_copy = name
	  @location = location


      if !Gem.available? @gem_to_copy
        puts "Gem unavailable - please install it"
        return
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
      g = get_gemspec name
      #puts "GemSpec #{g.full_gem_path}"
      l = g.full_gem_path
      d = File.join(l,"lib")
      #puts d
      d
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
      spec = get_gemspec @gem_to_copy
      #to be used in copying
      name =  spec.full_name
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
