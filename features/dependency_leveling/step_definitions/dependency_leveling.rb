require 'lib/nu/dependency_leveling/package_conflict_finder'
require 'lib/nu/dependency_leveling/package_conflict_overlap_resolver'

Given /^I am talking about the ([^"]*)$/ do |class_name|
	case class_name
	when "PackageConflictOverlapResolver"
		@leveler_maker = lambda do 
			def @packages.find(name)
				self.select{|spec| spec.name == name}
			end
			PackageConflictOverlapResolver.new(@installed_packages, @packages)
		end
	else
  	@leveler_maker = lambda{Kernel.const_get(class_name).new(@installed_packages)}		
	end

end

def existing_package(name, version)
	@packages.select {|spec| spec.name == name and spec.version.to_s == version}.first
end

Given /^package "([^"]*) \((\d\.\d\.\d)\)" is installed$/ do |name, version|
	@installed_packages ||= []
	@installed_packages << existing_package(name, version)
end

When /^package "([^"]*) \((\d\.\d\.\d)\)" is proposed$/ do |name, version|
	leveler = @leveler_maker.call
	@result = leveler.analyze_proposal(existing_package(name, version))
end


Then /^a conflict should be detected$/ do
  @result.conflict?.should be_true
end

Then /^a conflict should not be detected$/ do
  @result.conflict?.should be_false
end

Then /^the suggested version for package "([^"]*)" should be "([^"]*)"$/ do |name, version|
  row = @result.suggested_packages.select {|item| item[:name] == name }
  row.first[:version].to_s.should eql(version)
end

Then /^the conflicting package should be "([^"]*)" between "([^"]*)" and "([^"]*)"$/ do |name, version_one, version_two|
	Then "the conflicting package name should be \"#{name}\""
	Then "the conflicting packages should include \"#{name}\" between \"#{version_one}\" and \"#{version_two}\""
end

Then /^the conflicting packages should include "([^"]*)" between "([^"]*)" and "([^"]*)"$/ do |name, version_one, version_two|
	Then "the conflicting packages should include \"#{name}\""
	conflict = @result.conflicts.select{|item| item[:name] == name}.first
	conflict[:requirement_one].to_s.should satisfy{|v| v == version_one or v == version_two}
	conflict[:requirement_two].to_s.should satisfy{|v| v == version_one or v == version_two}
end

Then /^the conflicting package name should be "([^"]*)"$/ do |name|
	@result.conflicts.length.should eql(1)
  @result.conflicts.map{|item| item[:name]}.should include(name)
end

Then /^the conflicting packages should include "([^"]*)"$/ do |name|
  @result.conflicts.map{|item| item[:name]}.should include(name)
end
