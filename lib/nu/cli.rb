require 'FileUtils'
require 'thor'

module Nu
  class CLI < Thor
	include Thor::Actions
	
    desc "install GEMNAME", "installs a gem in the 'pwd'"
	method_options :location => :string
	
	def install(name)
		loc = options['location']
		loc = loc ? loc : 'lib'
		Nu::Loader.load name, loc
    end
	
	#def self.start
	#	Nu::Loader.load ARGV[1]
	#end
	
    
  end
end