require 'rubygems'

module Nu
	class GemTools
		
		def self.spec_for(name, requirement=nil)
			unless requirement.respond_to?('satisfied_by?') 
				requirement = Gem::Requirement.create(requirement)
			end
			dependency = Gem::Dependency.new(name,requirement)
			searcher = Gem::GemPathSearcher.new()
			all_installed_gems = searcher.init_gemspecs()
			
      return all_installed_gems.detect {|spec| spec.satisfies_requirement?(dependency)}
    end

		def self.lib_for(name, requirement=nil)
			gem = spec_for(name, requirement)
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
