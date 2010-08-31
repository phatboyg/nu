require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")
require File.expand_path(File.dirname(__FILE__) + "/api.rb")

class CliShim < HasOutAndLog
	
	def report(details=false)
		log "Report called."
		
		out "\n"
		out "The following packages are installed:"
		out "====================================="
		#TODO: render differently if details==true
		Nu::Api.report.each{|i| out "    #{i.name} (#{i.version})"}
		out "====================================="
		out ""
	end
	
	def install_package(package, package_version)
		Nu::Api.install_package(package, package_version)
		out "Installed package #{(package + " #{package_version}").strip}."
	end
	
	def get_setting(name)
		out "#{name} = #{Nu::Api.get_setting(name)}"
	end
end