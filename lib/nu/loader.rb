require 'rubygems'
require 'rubygems/dependency_installer'
require File.expand_path(File.dirname(__FILE__) + "/lib_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/gem_tools.rb")
require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")

module Nu
  class Loader < HasOutAndLog
    
		attr :gem_name
		attr :location
		attr :version
		
    def initialize(name, version, location, long_names, out, log)
      #improve the crap out of this
			@long_names = long_names
      @gem_name = name
      @location = location
      @version = version
			@lib_tools = Nu::LibTools.new
			@gem_tools = Nu::GemTools.new
			super(out, log)
    end

		def load_gem
			if (!Gem.available? @gem_name, @version) or (@version == nil)
        out "Getting #{(@gem_name + " #{@version}").strip}..."
        begin
          inst = Gem::DependencyInstaller.new
          inst.install @gem_name, @version
          inst.installed_gems.each do |spec|
            out "Got #{spec.full_name}"
          end
        rescue Gem::GemNotFoundException => e
          out "ERROR: #{e.message}"
          return false
        end
      else
        return true
      end
		end

		def copy_to_lib
			start_here = copy_source
			log "Copy From: #{start_here}"

			to = copy_dest
			log "Copy To: #{to}"

			FileUtils.copy_entry start_here, to
			@gem_tools.write_spec(gemspec, to)
		end

		def gemspec
			@gem_tools.spec_for(@gem_name, @version)
		end

    def copy_source
      @gem_tools.lib_for(@gem_name, @version).gsub '{lib}','lib'
    end

    def copy_dest
			@lib_tools.folder_for(gemspec, @location, @long_names)
    end

  end
end
