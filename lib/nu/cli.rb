require 'fileutils'
require 'thor'
require 'yaml'

module Nu
  class CLI < Thor
    include Thor::Actions

    def initialize(*)
      super

      @proj = Nu::Project.new
    end

    desc "install GEMNAME", "installs a gem in the 'pwd'"
	  method_options :location => :string, :version => :string

    def install(*names)

			loc = @proj.location
			cl = options['location']
			ver = options['version']
  		
      loc = cl unless cl.nil?

      names.each do |n|
				loader = Nu::Loader.new(n, loc, ver)
				if loader.load_gem
					 loader.copy_to_lib
				end
      end
    end

		desc "uninstall GEM", "remove the specified gem from the lib folder"
		def uninstall(gem)
			
		end

    desc "lib FOLDER", "where do you want to store the gems"
    def lib(folder)
      @proj.location = folder
    end

    desc "uselongnames", "turn the option of name + version number on"
    def uselongnames
      @proj.use_long_names
    end

    desc "useshortnames", "turn the option of name + version number off"
    def useshortnames
      @proj.use_short_names
    end

    def self.source_root
      File.dirname(__FILE__)
    end

  end
end
