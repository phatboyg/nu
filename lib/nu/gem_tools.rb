require 'rubygems'
require 'yaml'

module Nu
	class GemTools
		
		def dependency_from_requirement(spec, requirement)
			unless requirement.respond_to?('satisfied_by?') 
				requirement = Gem::Requirement.create(requirement)
			end
			Gem::Dependency.new(spec,requirement)
		end
		
		def find(package_name)
			dependency = dependency_from_requirement(package_name, nil)
			fetcher = Gem::SpecFetcher.new
			specs = fetcher.fetch(dependency, true)
			unless specs == nil
				specs.map! do |item| 
					item.first
				end
				specs.sort! {|a,b| b.version <=> a.version}
			end
			specs
		end
		
		def remote_spec_for(spec, requirement=nil)
			dependency = dependency_from_requirement(spec, nil)
			fetcher = Gem::SpecFetcher.new
			specs = fetcher.fetch(dependency, true)
			dependency = dependency_from_requirement(spec, requirement)
			unless specs == nil
				specs.map! do |item| 
					item.first
				end
				specs.sort! {|a,b| b.version <=> a.version}
				specs.detect {|spec| spec.satisfies_requirement?(dependency)}
			end
		end
		
		def spec_for(spec, requirement=nil)
			dependency = dependency_from_requirement(spec, requirement)
			searcher = Gem::GemPathSearcher.new()
			all_installed_gems = searcher.init_gemspecs()

      return all_installed_gems.detect {|spec| spec.satisfies_requirement?(dependency)}
    end

		def lib_for(name, requirement=nil)
			spec = spec_for(name, requirement)
			gem_path = spec.full_gem_path
			File.expand_path(File.join(gem_path,spec.require_paths.first))
		end

		def write_spec(spec, dest)
			dest = File.expand_path(dest)
			dest = File.join(dest,'nu_spec.yaml')
			
			unless spec.is_a?(Gem::Specification) 
				spec = spec_for(spec)
			end
			
			File.open( dest, 'w' ) do |out|
	 			YAML.dump(spec, out)
		  end
		end

	end
end
