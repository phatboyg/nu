
class DependencyLeveler
	def initialize(installed_packages)
		@installed_packages = installed_packages
	end
	
	def analyze_proposal(proposed_package)
		conflicts = find_conflicts(proposed_package)
		conflict_found = conflicts.length > 0
		
		proposed_packages = []
		proposed_packages = @installed_packages.map{|spec| {:name=> spec.name, :version=> spec.version.to_s}} 
		proposed_packages << {:name=>proposed_package.name, :version=> proposed_package.version.to_s}
		proposed_packages = proposed_packages | proposed_package.dependencies.map do |dep|
			{:name=>dep.name, :version=>dep.requirement.to_s}
		end
		proposed_packages.uniq!
		puts proposed_packages.inspect
		
		AnalysisResults.new do |r|
			r.conflict = conflict_found
			r.conflicting_packages = conflicts.map{|hash| hash[:name]}
			r.proposed_packages = proposed_packages
		end
	end
	
	def find_conflicts(proposed_spec)
		conflicts = []

		#installed packages that conflict with the proposed_spec
		conflicts = @installed_packages.select do |installed_package|
			installed_package.name == proposed_spec.name ? installed_package.version != proposed_spec.version : false
		end.map{|spec| {:name => spec.name, :version => spec.version}}
		
		#installed package's dependencies that conflict with the proposed_spec
		@installed_packages.each do |installed_package|
			conflicts = conflicts | installed_package.dependencies.select do |dep| 
				dep.name == proposed_spec.name ? !proposed_spec.satisfies_requirement?(dep) : false
			end.map{|dep| {:name => dep.name, :version => dep.requirements.to_s}}
		end

		#proposed_spec's dependencies that conflict with installed packages
		@installed_packages.each do |installed_package|
			conflicts = conflicts | proposed_spec.dependencies.select do |dep| 
				dep.name == installed_package.name ? !installed_package.satisfies_requirement?(dep) : false
			end.map{|dep| {:name => dep.name, :version => dep.requirement.to_s}}
		end

		conflicts
	end
	
	class AnalysisResults
		attr_accessor :conflict, :conflicting_packages, :proposed_packages
		alias_method :conflict?, :conflict
		
		def initialize(&block)
			yield(self)
		end		
	end
end
