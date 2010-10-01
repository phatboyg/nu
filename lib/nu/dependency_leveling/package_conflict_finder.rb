require 'rubygems'
require File.expand_path(File.dirname(__FILE__) + "/analysis_results.rb")

class PackageConflictFinder
	def initialize(installed_packages, package_lister)
		raise "package_lister must respond to find(name, version)" unless package_lister.respond_to?("find")
		@package_lister = package_lister
		@installed_packages = installed_packages
	end
	
	def analyze_proposal(proposed_package)
		suggested_packages = initialized_suggested_packages(proposed_package)
		
		conflicts = find_conflicts(proposed_package)
		conflict_found = conflicts.length > 0

		conflicting_packages = conflicts.map{|hash| hash[:name]}
		suggested_packages = suggested_packages.delete_if{|item| conflicting_packages.include?(item[:name])}

		AnalysisResults.new do |r|
			r.conflict = conflict_found
			r.conflicts = conflicts.uniq
			r.suggested_packages = suggested_packages.uniq
		end
	end
	
	private 
	
		def req(val)
			Gem::Requirement.create(val)
		end
	
		def initialized_suggested_packages(proposed_package)
			suggested_packages = @installed_packages.map{|spec| {:name=> spec.name, :version=> req(spec.version)}} 

			suggested_packages.reject! {|i| i[:name] == proposed_package.name}
			suggested_packages << {:name=>proposed_package.name, :version=> req(proposed_package.version)}  

			extract_dependencies(proposed_package, suggested_packages)
			
			return suggested_packages
		end
	
		def extract_dependencies(package, suggested_packages)
			package.dependencies.each do |dep|
				unless suggested_packages.any?{|i| i[:name] == dep.name}
					suggested_packages << {:name=>dep.name, :version=>dep.requirement}
					extract_dependencies(@package_lister.find(dep.name, dep.requirement), suggested_packages)
				end
			end
		end
	
		def find_conflicts(proposed_spec)
			conflicts = []

			#installed packages that conflict with the proposed_spec
			@installed_packages.each do |installed_package|
				if (installed_package.name == proposed_spec.name ? installed_package.version != proposed_spec.version : false)
					conflicts << {:name => installed_package.name, :requirement_one => req(installed_package.version), :requirement_two => req(proposed_spec.version)}
				end
			end
		
			#installed package's dependencies that conflict with the proposed_spec
			@installed_packages.each do |installed_package|
				conflicts = conflicts | installed_package.dependencies.select do |dep| 
					dep.name == proposed_spec.name ? !proposed_spec.satisfies_requirement?(dep) : false
				end.map{|dep| {:name => dep.name, :requirement_one => get_req_as_dep_of_installed(proposed_spec.name), :requirement_two => req(proposed_spec.version)}}
			end

			#proposed_spec's dependencies that conflict with installed packages
			@installed_packages.each do |installed_package|
				conflicts = conflicts | proposed_spec.dependencies.select do |dep| 
					dep.name == installed_package.name ? !installed_package.satisfies_requirement?(dep) : false
				end.map{|dep| {:name => dep.name, :requirement_one => dep.requirement, :requirement_two => get_req_as_dep_of_installed(installed_package.name)}}
			end

			conflicts
		end

		def get_req_as_dep_of_installed(name)
			
			deps = []
			@installed_packages.select{|p| p.dependencies.length > 0}.each do |spec| 
				spec.dependencies.each{|d| deps << d}
			end
			dep = deps.select{|d| d.name == name}.first
			return dep.requirement if dep
			return req(@installed_packages.select{|p| p.name == name}.first.version) unless dep
		end
end
