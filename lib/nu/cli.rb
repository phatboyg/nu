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
		@proj.ensure_default_config
		
  		loc = @proj.get_location
  		cl = options['location']
  		loc = cl unless cl.nil?
		
		names.each do |n|
			Nu::Loader.load n, loc
		end
      end
	
  	desc "lib FOLDER", "where do you want to store the gems"
  	def lib(folder)
  		@proj.set_location folder
  	end
	
  	def self.source_root
  		File.dirname(__FILE__)
  	end
	
  end
end