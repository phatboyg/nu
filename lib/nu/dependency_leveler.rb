
class DependencyLeveler
	def initialize(installed_packages)
		@installed_packages = installed_packages
	end
	
	def analyze_proposal(proposed_package)
		AnalysisResults.new
	end
	
	class AnalysisResults
		def conflict?
			
		end
		
		def acceptable_packages
			{:name => '', :version => ''}
		end
	end
end
