resource "aws_dynamodb_table" "personapi_dynamodb_table" {
    name                  = "Person"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "personId"

    attribute {
        name              = "personId"
        type              = "S"
    }

    tags = {
        Name              = "person-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
    }
}

resource "aws_iam_policy" "personapi_dynamodb_table_policy" {
    name                  = "Lambda_DynamoDB_PersonAPI"
    description           = "A policy allowing read/write operations on person dynamoDB for the person API"
    path                  = "/person-api"

    policy                = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                        "dynamodb:BatchGetItem",
                        "dynamodb:GetItem",
                        "dynamodb:Query",
                        "dynamodb:Scan",
                        "dynamodb:BatchWriteItem",
                        "dynamodb:PutItem",
                        "dynamodb:UpdateItem"
                     ],
            "Resource": "${aws_dynamodb_table.personapi_dynamodb_table.arn}"
        }
    ]
}
EOF
}
