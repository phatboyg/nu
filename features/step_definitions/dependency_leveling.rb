
Given /^package "([^"]*) \((\d\.\d\.\d)\)" is installed$/ do |name, version|
  package = @packages.select {|spec| spec.name == name && spec.version == version}.first
	@installed_packages ||= [] << package
end


When /^package "([^"]*) \((\d\.\d\.\d)\)" is proposed$/ do |name, version|
  pending # express the regexp above with the code you wish you had
end


Then /^a conflict should be detected$/ do
  pending # express the regexp above with the code you wish you had
end

Then /^a conflict should not be detected$/ do
  pending # express the regexp above with the code you wish you had
end

Then /^the acceptable version for package "([^"]*)" should be "(\d\.\d\.\d)"$/ do |name, version|
  pending # express the regexp above with the code you wish you had
end

Then /^the conflicting package name should be "([^"]*)"$/ do |name|
  pending # express the regexp above with the code you wish you had
end

Then /^the conflicting packages should include "([^"]*)"$/ do |name|
  pending # express the regexp above with the code you wish you had
end
