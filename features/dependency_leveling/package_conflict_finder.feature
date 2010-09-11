Feature: Package Conflict Finder
As a developer
In order to help eliminate dependency hell
I want Nu to automatically detect dependency version collisions

Background:
	Given I am talking about the PackageConflictFinder
	And package "nhibernate (2.1.2)" exists and depends on:
		| name			| constraint |
		| castle.core	| 1.1.0		 |
	And package "castle.windsor (2.5.0)" exists and depends on:
	 	| name			| constraint |
		| castle.core	| 2.5.0		 |
	And package "castle.windsor (2.0.0)" exists and depends on:
		| name			| constraint |
		| castle.core	| 1.1.0		 |
	And package "castle.core (1.1.0)" exists
	And package "castle.core (2.5.0)" exists

Scenario: News Group Example
	Given package "nhibernate (2.1.2)" is installed
	And package "castle.core (1.1.0)" is installed
	When package "castle.windsor (2.5.0)" is proposed
	Then a conflict should be detected
	And the conflicting package should be "castle.core" between "= 1.1.0" and "= 2.5.0"

Scenario: No Conflict
	Given package "nhibernate (2.1.2)" is installed
	And package "castle.core (1.1.0)" is installed
	When package "castle.windsor (2.0.0)" is proposed
	Then a conflict should not be detected
	And the suggested version for package "nhibernate" should be "= 2.1.2"
	And the suggested version for package "castle.core" should be "= 1.1.0"
	And the suggested version for package "castle.windsor" should be "= 2.0.0"

Scenario: Conflict with a directly installed package
	Given package "castle.core (1.1.0)" is installed
	When package "castle.windsor (2.5.0)" is proposed
	Then a conflict should be detected
	And the conflicting package name should be "castle.core"
	
Scenario: No conflict with a directly installed package
	Given package "castle.core (1.1.0)" is installed
	When package "castle.windsor (2.0.0)" is proposed
	Then a conflict should not be detected
	And the suggested version for package "castle.core" should be "= 1.1.0"
	And the suggested version for package "castle.windsor" should be "= 2.0.0"
	
Scenario: Multiple conflicts
	Given package "has_conflict_1 (1.0.0)" exists and depends on:
		| name			| constraint |
		| castle.core	| 1.1.0		 |
		| dependency_1	| 1.0.0		 |
	And package "dependency_1 (1.0.0)" exists
	And package "dependency_1 (2.0.0)" exists
	And package "has_conflict_2 (1.0.0)" exists and depends on:
		| name			| constraint |
		| castle.core	| 2.5.0		 |
		| dependency_1	| 2.0.0		 |
	And package "has_conflict_1 (1.0.0)" is installed
	And package "castle.core (1.1.0)" is installed
	And package "dependency_1 (1.0.0)" is installed
	When package "has_conflict_2 (1.0.0)" is proposed
	Then a conflict should be detected
	And the conflicting packages should include "castle.core"
	And the conflicting packages should include "dependency_1" between "= 1.0.0" and "= 2.0.0"	
	
Scenario: No conflict because installed satisfies proposed's dependencies
	Given package "easy_going (1.0.0)" exists and depends on:
		| name			| constraint |
		| castle.core	| >=0.0.0	 |
	And package "castle.core (1.1.0)" is installed
	When package "easy_going (1.0.0)" is proposed
	Then a conflict should not be detected
	And the suggested version for package "castle.core" should be "= 1.1.0"
	And the suggested version for package "easy_going" should be "= 1.0.0"