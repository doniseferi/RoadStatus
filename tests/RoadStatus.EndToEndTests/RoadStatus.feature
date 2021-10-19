Feature: Road Status
	Get the status of a road that falls within the TfL network

Scenario: The dislay name of a road is displayed
	Given a valid road ID is specified:
		| Road Id | Display Name |
		| A1      | A1           |
		| A2      | A2           |
		| A20     | A20          |
		| A2      | A2           |
	When the client is run
	Then the road displayName should be displayed

Scenario: The status of a road is displayed
	Given a valid road ID is specified:
		| Road Id |
		| A1      |
		| A2      |
		| A20     |
		| A2      |
	When the client is run
	Then the road 'statusSeverity' should be displayed as 'Road Status'

Scenario: A description of the road status is displayed
	Given a valid road ID is specified:
		| Road Id |
		| A1      |
		| A2      |
		| A20     |
		| A2      |
	When the client is run
	Then the road 'statusSeverityDescription' should be displayed as 'Road Status Description'

Scenario: An informitive error is displayed on invalid input
	Given an invalid road ID is specified:
		| Road Id |
		| A404    |
		| A123    |
		| A99     |
		| Abc     |
	When the client is run
	Then the application should return an informative error

Scenario: The application exits with a non-zero code on invalid input
	Given an invalid road ID is specified:
		| Road Id |
		| A404    |
		| A123    |
		| A99     |
		| Abc     |
	When the client is run
	Then the application should exit with a non-zero System Error code