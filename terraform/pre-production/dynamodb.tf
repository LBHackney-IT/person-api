resource "aws_dynamodb_table" "personapi_dynamodb_table" {
  name         = "Persons"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "id"

  attribute {
    name = "id"
    type = "S"
  }

  tags = merge(
    local.default_tags,
    {
      BackupPolicy = "Dev", Backup = false, Confidentiality = "Internal"
    }
  )

  point_in_time_recovery {
    enabled = false
  }
}

resource "aws_dynamodb_table" "refGenerator_dynamodb_table" {
  name         = "RefGenerator"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "refName"

  attribute {
    name = "refName"
    type = "S"
  }

  tags = merge(
    local.default_tags,
    {
      BackupPolicy = "Dev", Backup = false, Confidentiality = "Internal"
    }
  )

  point_in_time_recovery {
    enabled = false
  }
}
