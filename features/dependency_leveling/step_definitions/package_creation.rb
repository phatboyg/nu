require 'rubygems'

Given /^package "([^"]*) \((\d\.\d\.\d)\)" exists and depends on:$/ do |name, version, table|
  # table is a Cucumber::Ast::Table
	spec = create_package(name, version)
	table.hashes.each do |hash|
		constraints = hash['constraint'].split(',')
		spec.add_dependency(hash['name'], constraints)
	end
end

Given /^package "([^"]*) \((\d\.\d\.\d)\)" exists$/ do |name, version|
	create_package(name, version)
end

def create_package(name, version)
	spec = Gem::Specification.new do |s|
	    s.name = name
	    s.version = version
	    s.summary = 'Totally fake gem specification...'
	end
	@packages << spec
	spec
end
