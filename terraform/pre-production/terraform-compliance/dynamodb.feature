Feature: DynamoDB is used as our NoSQL database service
  In order to improve security
  As engineers
  We'll use ensure our DynamoDB tables are configured correctly

  Scenario: Ensure BackupPolicy tag is present
    Given I have aws_dynamodb_table defined
    Then it must contain tags
    And it must contain BackupPolicy

  Scenario: Ensure point in time recovery disabled
    Given I have aws_dynamodb_table defined
    Then it must contain point_in_time_recovery
    And its enabled property must be false

  Scenario: Ensure a maximum of 2 GSIs
    Given I have aws_dynamodb_table defined
    When it contains global_secondary_index
    When I count them
    Then I expect the result is less and equal to 2
