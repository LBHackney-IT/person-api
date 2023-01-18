# INSTRUCTIONS:
# 1) ENSURE YOU POPULATE THE LOCALS
# 2) ENSURE YOU REPLACE ALL INPUT PARAMETERS, THAT CURRENTLY STATE 'ENTER VALUE', WITH VALID VALUES
# 3) YOUR CODE WOULD NOT COMPILE IF STEP NUMBER 2 IS NOT PERFORMED!
# 4) ENSURE YOU CREATE A BUCKET FOR YOUR STATE FILE AND YOU ADD THE NAME BELOW - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL - FOR APIS, THE BUCKETS ALREADY EXIST
# 5) THE VALUES OF THE COMMON COMPONENTS THAT YOU WILL NEED ARE PROVIDED IN THE COMMENTS
# 6) IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
# 7) ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY

terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.0"
    }
  }
}

provider "aws" {
  region = "eu-west-2"
}

data "aws_caller_identity" "current" {}

data "aws_region" "current" {}

locals {
  parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
  default_tags = {
    Name              = "person-api-${var.environment_name}"
    Environment       = var.environment_name
    terraform-managed = true
    project_name      = var.project_name
  }
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-housing-production"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/person-api/state"
  }
}

resource "aws_sns_topic" "person_topic" {
  name                        = "person.fifo"
  fifo_topic                  = true
  content_based_deduplication = true
  kms_master_key_id           = "alias/aws/sns"
  sqs_success_feedback_role_arn = "arn:aws:iam::${data.aws_caller_identity.current.account_id}:role/LBH_SNS_DELIVERY_LOGGING_ROLE"
  sqs_success_feedback_sample_rate = "100"
  sqs_failure_feedback_role_arn = "arn:aws:iam::${data.aws_caller_identity.current.account_id}:role/LBH_SNS_DELIVERY_LOGGING_ROLE"
}

resource "aws_ssm_parameter" "person_sns_arn" {
  name  = "/sns-topic/production/person/arn"
  type  = "String"
  value = aws_sns_topic.person_topic.arn
}

module "person_api_cloudwatch_dashboard" {
  source              = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/dashboards/api-dashboard"
  environment_name    = var.environment_name
  api_name            = "person-api"
  sns_topic_name      = aws_sns_topic.person_topic.name
  dynamodb_table_name = aws_dynamodb_table.personapi_dynamodb_table.name
}

data "aws_ssm_parameter" "cloudwatch_topic_arn" {
  name = "/housing-tl/${var.environment_name}/cloudwatch-alarms-topic-arn"
}

module "api-alarm" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/api-alarm"
  environment_name = var.environment_name
  api_name         = "person-api"
  alarm_period     = "300"
  error_threshold  = "1"
  sns_topic_arn    = data.aws_ssm_parameter.cloudwatch_topic_arn.value
}
    
module "sns-delivery-failure-alarm" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/sns-delivery-metric-and-alarm"
  environment_name = var.environment_name
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
  sns_topic_name   = "person.fifo"
  sns_topic_arn_for_notifications = data.aws_ssm_parameter.cloudwatch_topic_arn.value
}

resource "aws_sns_topic_policy" "default" {
  arn = aws_sns_topic.person_topic.arn

  policy = data.aws_iam_policy_document.sns_topic_policy.json
}

data "aws_ssm_parameter" "prod_account_id" {
  name = "/prod-apis/account-id"
}

data "aws_iam_policy_document" "sns_topic_policy" {
  policy_id = "__default_policy_ID"
  statement {
      actions = [
        "sns:GetTopicAttributes",
        "sns:SetTopicAttributes",
        "sns:AddPermission",
        "sns:RemovePermission",
        "sns:DeleteTopic",
        "sns:Subscribe",
        "sns:ListSubscriptionsByTopic",
        "sns:Publish"
      ]

      condition {
        test     = "StringEquals"
        variable = "AWS:SourceOwner"

        values = [
          data.aws_caller_identity.current.account_id
        ]

      }

      effect = "Allow"

      principals {
        type        = "AWS"
        identifiers = ["*"]
      }

      resources = [
        aws_sns_topic.person_topic.arn
      ]

      sid = "__default_statement_ID"
    }
  statement {
      actions = [
        "sns:Subscribe"
      ]

      effect = "Allow"

      principals {
        type        = "AWS"
        identifiers = ["arn:aws:iam::${data.aws_ssm_parameter.prod_account_id.value}:role/LBH_Circle_CI_Deployment_Role"]
      }
      resources = [
        aws_sns_topic.person_topic.arn
      ]

      sid = "prod-statement"
    }
}	

  

