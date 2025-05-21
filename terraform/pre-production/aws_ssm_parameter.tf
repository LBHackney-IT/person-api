resource "aws_ssm_parameter" "app_config_retrieval_role_arn" {
  name  = "/housing-tl/pre-production/app-config/retrieval-role-arn"
  type  = "String"
  value = "to_be_set_manually"

  lifecycle {
    ignore_changes = [
      value,
    ]
  }
}

resource "aws_ssm_parameter" "person_api_configuration_data" {
  name  = "/housing-tl/person-api/configurationData"
  type  = "String"
  value = "to_be_set_manually"

  lifecycle {
    ignore_changes = [
      value,
    ]
  }
}

resource "aws_ssm_parameter" "pact_broker_password" {
  name  = "/contract-testing/pre-production/pact-broker-password"
  type  = "String"
  value = "to_be_set_manually"

  lifecycle {
    ignore_changes = [
      value,
    ]
  }
}
