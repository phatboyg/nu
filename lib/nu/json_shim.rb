require 'rubygems'
gem 'activesupport', '=2.3.5'
require 'active_support'
require File.expand_path(File.dirname(__FILE__) + "/has_out_and_log.rb")

class JsonShim < HasOutAndLog

	def report
		out Nu::Api.report.to_json
	end
	
	def install_package(package, package_version)
		Nu::Api.install_package(package, package_version)
	end
	
	def get_setting(name)
		out ({:name => name, :value => Nu::Api.get_setting(name) }.to_json)
	end
	
end