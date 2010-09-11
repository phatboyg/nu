class AnalysisResults
	attr_accessor :conflict, :conflicts, :suggested_packages
	alias_method :conflict?, :conflict
	
	def initialize(&block)
		yield(self)
	end		
end