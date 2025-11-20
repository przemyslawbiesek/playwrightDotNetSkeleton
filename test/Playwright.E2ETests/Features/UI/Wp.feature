Feature: Wp simple test

@wp
Scenario: User can access WP Poczta login page from WP.pl and verify elements
Given I navigate to WP.pl homepage
When I click on the Poczta link
Then I should see the WP Poczta login page
And I should see the email address field
And I should see the password field
And I should see the login button
And I should see the create new account link
When I click on the create new account link
Then I should see the registration page
