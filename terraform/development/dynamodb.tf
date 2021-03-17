resource "aws_dynamodb_table" "personapi_dynamodb_table" {
    name           = "Person"
    billing_mode   = "PROVISIONED"
    read_capacity  = 10
    write_capacity = 10
    hash_key       = "personId"

    attribute {
        name = "personId"
        type = "S"
    }

    tags = {
        Name = "person-api-${var.environment_name}"
        Environment = var.environment_name
        terraform-managed = true
        project_name = var.project_name
    }
}
