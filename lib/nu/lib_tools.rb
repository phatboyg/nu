require 'rubygems'

module Nu
	class LibTools
		
		def folder_for(spec, lib, long_name=false)
			lib = File.expand_path(lib)
      if long_name
        name = spec.full_name
      else
        name = spec.name
      end

      to = "#{lib}/#{name}"
			if Dir[to] == [] #may need a smarter guy here
	      FileUtils.mkpath to
      end

			return to
		end
		
		def read_specs_from_lib(lib)
			lib = File.expand_path(lib)
			glob = "#{lib}/**/nu_spec.yaml"
			files = Dir.glob(glob)
			files.map{|file| YAML::load_file(file)}
		end
		
	end
end