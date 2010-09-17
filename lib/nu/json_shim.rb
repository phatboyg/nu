require 'rubygems'
gem 'activesupport', '=2.3.5'
require 'active_support'
require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")

class JsonShim < HasOutAndLog

	def report
		json_out Nu::Api.report
	end
	
	def json_out(object)
		out object.to_json
	end
	
	def install_package(package, package_version)
		Nu::Api.install_package(package, package_version)
	end
	
	def get_setting(name)
		json_out ({:name => name, :value => Nu::Api.get_setting(name) })
	end
	
	def specification(name, version, source)
		log "Json Shim Specification called. name: #{name} version: #{version} source:#{source}"
		if version == nil
			json_out Nu::Api.retrieve_specification(name, :from => source)
		else
			json_out Nu::Api.retrieve_specification_with_version(name, version, :from => source)
		end
	end
	
	def propose(name, version)
		log "Json Shim Propose called. name: #{name} version: #{version}"
		json_out Nu::Api.propose_package(name, version)
	end
	
end