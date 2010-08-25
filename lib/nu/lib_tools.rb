require 'rubygems'

module Nu
	class LibTools
		
		def self.gemspec_for(name)
      gems = Gem.source_index.find_name name
      return gems.last if gems.length > 0
    end

		def self.folder_name_for(gem_name, lib, long_name=false)
			  spec = gemspec_for(gem_name)

	      if long_name
	        name = spec.full_name
	      else
	        name = spec.name
	      end
	      Dir.pwd + "/#{lib}/#{name}"
		end
	end
end