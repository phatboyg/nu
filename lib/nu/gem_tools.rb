require 'rubygems'

module Nu
	class GemTools
		
		def self.spec_for(name)
      gems = Gem.source_index.find_name name
      return gems.last if gems.length > 0
    end

		def self.lib_for(name)
			gem = spec_for(name)
			gem_path = gem.full_gem_path
			libdir = File.join(gem_path,"lib")
			unless File.exist?(libdir)
			  libdir = IO.readlines(File.join(gem_path, ".require_paths"))[0].strip 
			  libdir = File.expand_path(File.join(gem_path,libdir))
			end
			libdir
		end

	end
end