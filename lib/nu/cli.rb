require 'FileUtils'
require 'thor'
require 'yaml'

module Nu
  class CLI < Thor
    include Thor::Actions
	
	  def initialize(*)
		  super
		  @config_file = ".nu/options.yaml"
	  end
	
    desc "install GEMNAME", "installs a gem in the 'pwd'"
	  method_options :location => :string
	
	  def install(name)
		  ensure_default_config
		
		  loc = get_location
		  cl = options['location']
		  loc = cl unless cl.nil?
		
		  Nu::Loader.load name, loc
    end
	
	  desc "lib FOLDER", "where do you want to store the gems"
	  def lib(folder)
		  File.delete @config_file if File.exist? @config_file
		  add_file @config_file
		  content = YAML::dump( { 'lib' => folder })
		  append_file @config_file, content
	  end
	
	  def self.source_root
		  File.dirname(__FILE__)
	  end
	
  private
	  def ensure_default_config
		  if File.exist? @config_file
			  return
		  end
		  add_file @config_file
		  content = YAML::dump( {'lib'=>'lib'} )
		  append_file @config_file, content
	  end
	
	  def get_location
		  content = YAML.load_file @config_file
		  content['lib']
	  end
	
  end
end
