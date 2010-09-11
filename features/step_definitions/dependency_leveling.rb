require 'lib/nu/dependency_leveler'

def existing_package(name, version)
	@packages.select {|spec| spec.name == name and spec.version.to_s == version}.first
end

Given /^package "([^"]*) \((\d\.\d\.\d)\)" is installed$/ do |name, version|
	@installed_packages ||= []
	@installed_packages << existing_package(name, version)
end

When /^package "([^"]*) \((\d\.\d\.\d)\)" is proposed$/ do |name, version|
	leveler = DependencyLeveler.new(@installed_packages)
	@result = leveler.analyze_proposal(existing_package(name, version))
end


Then /^a conflict should be detected$/ do
  @result.conflict?.should be_true
end

Then /^a conflict should not be detected$/ do
  @result.conflict?.should be_false
end

Then /^the proposed version for package "([^"]*)" should be "(\d\.\d\.\d)"$/ do |name, version|
  row = @result.proposed_packages.select {|item| item[:name] == name }
  row.first[:version].to_s.should eql(version)
end

Then /^the conflicting package name should be "([^"]*)"$/ do |name|
  @result.conflicting_packages.should include(name)
end

Then /^the conflicting packages should include "([^"]*)"$/ do |name|
  @result.conflicting_packages.should include(name)
end
