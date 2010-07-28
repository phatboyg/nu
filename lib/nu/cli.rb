require 'FileUtils'
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
    method_options :location => :string

    def install(*names)

      loc = @proj.location
      cl = options['location']
      loc = cl unless cl.nil?

      names.each do |n|
        Nu::Loader.load n, loc
      end
    end

    desc "lib FOLDER", "where do you want to store the gems"
    def lib(folder)
      @proj.location= folder
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
