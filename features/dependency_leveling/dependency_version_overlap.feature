Feature: Dependency Version Overlap Conflict Resolution
As a developer
In order to help eliminate dependency hell
I want Nu to automatically resolve dependency version collisions

Background:
	Given I am talking about the PackageConflictOverlapResolver

Scenario: No conflict because of forgiving constraint range, downgrade a dependency
	Given package "dependency (1.0.0)" exists
	And package "dependency (1.0.1)" exists
	And package "top (1.0.0)" exists and depends on:
		| name			| constraint 		|
		| dependency	| >=1.0.0, <1.1.0	|
	And package "proposed (1.0.0)" exists and depends on:
		| name			| constraint 		|
		| dependency	| 1.0.0				|
	And package "top (1.0.0)" is installed
	And package "dependency (1.0.1)" is installed
	When package "proposed (1.0.0)" is proposed
	Then a conflict should not be detected
	And the suggested version for package "top" should be "= 1.0.0"
	And the suggested version for package "proposed" should be "= 1.0.0"
	#this is the one that should be downgraded from 1.0.1
	And the suggested version for package "dependency" should be "= 1.0.0" 
	
Scenario: Multiple potential dependency versions acceptable, choose the highest
	Given package "dep (1.0.0)" exists
	And package "dep (1.0.3)" exists
	And package "dep (1.0.1)" exists
	And package "dep (1.0.2)" exists
	And package "dep (1.1.0)" exists
	And package "top (1.0.0)" exists and depends on:
		| name		| constraint |
		| dep		| >=1.0.0	 |
	And package "top (1.0.0)" is installed
	And package "dep (1.0.0)" is installed
	And package "proposed (1.0.0)" exists and depends on:
		| name		| constraint 		|
		| dep		| >= 1.0.1, < 1.1.0 |
	When package "proposed (1.0.0)" is proposed
	Then a conflict should not be detected
	And the suggested version for package "dep" should be "= 1.0.3"
	
Scenario: Conflict is with a package that is not a dependency itself in the lib (this would have been requested explicitly)
	Given package "explicit (1.0.0)" exists
	And package "explicit (1.0.1)" exists
	And package "explicit (1.0.0)" is installed
	And package "interloper (2.0.0)" exists and depends on:
		| name		| constraint 			|
		| explicit  | >= 1.0.1, <= 1.4.0	|
	When package "interloper (2.0.0)" is proposed
	Then a conflict should be detected
	And the conflicting package should be "explicit" between "= 1.0.0" and ">= 1.0.1, <= 1.4.0"
	