Feature: NewUserPermissions
	User that has been created recently
	should have got the permissions to the
	common actions.

@mytag
Scenario: User should be able to create blog post
	Given User is created and activated
	And Blog "Test" is created
	When User creates blog post in blog "Test"
	Then Post should be crated in blog "Test"
