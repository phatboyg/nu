require 'rubygems'
require File.expand_path(File.dirname(__FILE__) + "/package_conflict_finder.rb")

class PackageConflictOverlapResolver
	def initialize(installed_packages, package_lister)
		@installed_packages = installed_packages
		raise "package_lister must respond to find(name)" unless package_lister.respond_to?("find")
		@package_lister = package_lister
		@conflict_finder = PackageConflictFinder.new(installed_packages)
	end
	
	def analyze_proposal(proposed_package)
		r = @conflict_finder.analyze_proposal(proposed_package)
		
		Array.new(r.conflicts).each do |conflict|
			hav = highest_acceptable_version(conflict[:name], conflict[:requirement_one], conflict[:requirement_two])
			resolve_conflict(r,conflict,hav) if hav
		end if r.conflict?

		r.suggested_packages.uniq!
		r.conflicts.uniq!
		
		return r
	end
	
	def highest_acceptable_version(name, requirement_one, requirement_two)
		candidates = @package_lister.find(name)
		candidates = candidates.select do |spec| 
			requirement_one.satisfied_by?(spec.version) and requirement_two.satisfied_by?(spec.version)
		end
		return Gem::Requirement.new(candidates.sort{|a,b| a.version <=> b.version}.last.version) if candidates.length > 0
	end
	
	private
	
		def resolve_conflict(results,conflict,version)
			results.conflicts.reject! {|c| c[:name] == conflict[:name]}
			results.suggested_packages.reject! {|p| p[:name] == conflict[:name]}
			results.suggested_packages << {:name => conflict[:name], :version => version}
			results.conflict = results.conflicts.length > 0
		end
		
end