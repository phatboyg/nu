require 'rubygems'

module Nu
	class LibTools
		
		def self.folder_for(spec, lib, long_name=false)
      if long_name
        name = spec.full_name
      else
        name = spec.name
      end

      to = Dir.pwd + "/#{lib}/#{name}"
			if Dir[to] == [] #may need a smarter guy here
	      FileUtils.mkpath to
      end

			return to
		end
		
		def self.read_specs_from_lib(lib)
			lib = File.expand_path(lib)
			glob = "#{lib}/**/nu_spec.yaml"
			files = Dir.glob(glob)
			files.map{|file| YAML::load_file(file)}
		end
		
	end
end