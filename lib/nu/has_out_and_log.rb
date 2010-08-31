
class HasOutAndLog
	def initialize(out, log)
		@out = out
		@log = log
	end
	
	protected
	
	def out(msg)
		@out.call(msg)
	end
	
	def log(msg)
		@log.call(msg)
	end
end