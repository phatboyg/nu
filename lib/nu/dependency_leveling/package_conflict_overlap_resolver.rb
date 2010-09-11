require 'rubygems'
require File.expand_path(File.dirname(__FILE__) + "/package_conflict_finder.rb")

class PackageConflictOverlapResolver
	def initialize(installed_packages)
		@installed_packages = installed_packages
		@conflict_finder = PackageConflictFinder.new(installed_packages)
	end
	
	def analyze_proposal(proposed_package)
		r = @conflict_finder.analyze_proposal(proposed_package)
		adapt_requirements(r.conflicts)
		
		Array.new(r.conflicts).each do |conflict|
			if conflict[:requirement_one].encompasses?(conflict[:requirement_two])
				resolve_conflict(r,conflict,conflict[:requirement_one])
			else 
				if conflict[:requirement_two].encompasses?(conflict[:requirement_one])
					resolve_conflict(r,conflict,conflict[:requirement_two])
				end
			end
		end if r.conflict?
		
		r
	end
	
	private
	
		def resolve_conflict(results,conflict,selected_requirement)
			results.conflicts.reject! {|c| c[:name] == conflict[:name]}
			results.suggested_packages << {:name => conflict[:name], :version => selected_requirement}
			results.conflict = results.conflicts.length > 0
		end
		
		def adapt_requirements(conflicts)
			conflicts.each do |c| 
				c[:requirement_one] = add_methods_to_req(c[:requirement_one])
				c[:requirement_two] = add_methods_to_req(c[:requirement_two])
			end
		end
		
		def add_methods_to_req(req)
			def req.encompasses?(requirement)
				false #TODO: Make this method!
			end
			req
		end
end