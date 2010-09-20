
class HasOutAndLog
	def initialize(out, log)
		@out = out
		@log = log
	end
	
	protected
	
	def out(msg)
		@out.call(msg)
	end
	
	def hr(suffix=nil)
		line = "=" * 80
		out(line) unless suffix
		out(line + suffix) if suffix
	end
	
	def log(msg)
		@log.call(msg)
	end
end