require 'fileutils'

module Nu
  class Project

    def initialize
      @config_file = ".nu/options.yaml"
      ensure_default_config
    end

    # need to meta awesome this
    def location
      opts = get_options
      opts['lib']
    end

    # need to meta awesome this
    def location=(name)
      opts = get_options
      opts['lib']=name
      save_file(opts)
      nil
    end

    #need to meta awesome this
    def should_use_long_names?
      opts = get_options
      opts['uselongnames']
    end

    #need to meta awesome this
    def use_long_names
      opts = get_options
      opts['uselongnames']=true
      save_file(opts)
      nil
    end

    #need to meta awesome this
    def use_short_names
      opts = get_options
      opts['uselongnames']=false
      save_file(opts)
      nil
    end

    def add_file(name, content)
      FileUtils.mkdir_p(File.dirname(name))
      File.open(name, 'w'){|f| f.write(content)}
    end

    private
    def ensure_default_config
      if File.exist? @config_file
        return
      end

      save_file( {'lib'=>'lib','uselongnames'=>false} )
    end

    def get_options
      YAML.load_file @config_file
    end

    def save_file(opts)
      content = YAML::dump(opts)
      File.delete @config_file if File.exist? @config_file
      dir_name = File.dirname(@config_file)
      FileUtils.mkdir_p(dir_name) unless File.exist? dir_name
      File.open(@config_file, 'w') {|f| f.write(content) }
    end

  end
end
