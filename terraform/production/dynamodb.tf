resource "aws_dynamodb_table" "personapi_dynamodb_table" {
  name           = "Persons"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "id"

  attribute {
    name = "id"
    type = "S"
  }

  tags = merge(
    local.default_tags,
    { BackupPolicy = "Prod" }
  )

  point_in_time_recovery {
    enabled = true
  }
}
