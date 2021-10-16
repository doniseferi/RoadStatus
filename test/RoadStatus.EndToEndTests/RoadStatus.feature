Feature: Road Status
	Get the status of a road that falls within the TfL network

Scenario: The dislay name of a road is displayed
	Given a valid road ID is specified:
		| Road Id | Display Name          |
		| a1      | A1                    |
		| a2      | A2                    |
		| a20     | A20                   |
		| a406    | North Circular (A406) |
		| a2      | A2                    |
	When the client is run
	Then the road displayName should be displayed

Scenario: The status of a road is displayed
	Given a valid road ID is specified:
		| Road Id |
		| a1      |
		| a2      |
		| a20     |
		| a406    |
		| a2      |
	When the client is run
	Then the road 'statusSeverity' should be displayed as 'Road Status'

Scenario: A discription of the road status is displayed
	Given a valid road ID is specified:
		| Road Id |   
		| Road Id |
		| a1      |
		| a2      |
		| a20     |
		| a406    |
		| a2      |
	When the client is run
	Then the road 'statusSeverityDescription' should be displayed as 'Road Status Description'

Scenario: An informitive error is displayed on invalid input
	Given an invalid road ID is specified:
		| Road Id |
		| a404    |
		| a123    |
		| a99     |
		| abc     |
	When the client is run
	Then the application should return an informative error

Scenario: The application exits with a non-zero code on invalid input
	Given an invalid road ID is specified:
		| Road Id |
		| a404    |
		| a123    |
		| a99     |
		| abc     |
	When the client is run
	Then the application should exit with a non-zero System Error code