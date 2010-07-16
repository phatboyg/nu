require 'FileUtils'

module Nu
  class CLI
    
    def self.start

      #improve the crap out of this
      action = ARGV[0]
      @gem_to_copy = ARGV[1]


      l = Nu::Loader.new
      if !l.available? @gem_to_copy
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
      #files = get_files()
      #files.each do |file|
        #someone please show me how to do this better
       # from = File.expand_path(file, start_here+"/..")
        #puts "copy #{from} #{to}"
        #FileUtils.copy(from, to)
      #end
    end
    
    def self.get_copy_from
      l = Nu::Loader.new
      libdir = l.get_libdir @gem_to_copy
      #puts File.expand_path libdir
	  #try Dir.glob("{bin,lib}/**/*")
      libdir.gsub '{lib}','lib'
    end
    
    def self.get_files
      l = Nu::Loader.new
      spec = l.get_gemspec @gem_to_copy
      files = spec.lib_files #get full path
      files
    end
    
    def self.get_copy_to
      l = Nu::Loader.new
      spec = l.get_gemspec @gem_to_copy
      #to be used in copying
      name =  spec.full_name
      to = Dir.pwd + "/lib/#{name}"
      if Dir[to] == nil
        FileUtils.mkdir to
      end
      to
    end
  end
end