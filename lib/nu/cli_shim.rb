require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")
require File.expand_path(File.dirname(__FILE__) + "/api.rb")

class CliShim < HasOutAndLog
	
	def report(details=false)
		log "Report called."
		
		out "\n"
		out "The following packages are installed:"
		hr
		#TODO: render differently if details==true
		Nu::Api.report.each{|i| out "    #{i.name} (#{i.version})"}
		hr("\n")
	end
	
	def install_package(package, package_version)
		Nu::Api.install_package(package, package_version)
		out "Installed package #{(package + " #{package_version}").strip}."
	end
	
	def get_setting(name)
		out "#{name} = #{Nu::Api.get_setting(name)}"
	end
	
	def propose(name, version)
		log "Propose called. name: #{name} version: #{version}"
		report(true)
		out "Analyzing effects of installing #{name}..."
		results = Nu::Api.propose_package(name, version)
		
		if results.conflict?
			out "A conflict is detected between #{name} \nand the currently installed:"
			hr
			results.conflicts.each{|spec| out "    #{spec[:name]} (#{spec[:requirement_one].requirements.first}) vs. (#{spec[:requirement_two].requirements.first})"}
			hr("\n")
		else
			out "No unresolved conflicts. \nAdding #{name} would result in:"
			hr
			results.suggested_packages.each{|spec| out "    #{spec[:name]} (#{spec[:version]})"}
			hr("\n")
		end
	end
	
end