resource "aws_dynamodb_table" "personapi_dynamodb_table" {
    name                  = "Persons"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "id"

    attribute {
        name              = "id"
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
    name                  = "lambda-dynamodb-person-api"
    description           = "A policy allowing read/write operations on person dynamoDB for the person API"
    path                  = "/person-api/"

    policy                = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                        "dynamodb:BatchGet*",
                        "dynamodb:BatchWrite*",
                        "dynamodb:DeleteItem",
                        "dynamodb:DescribeStream",
                        "dynamodb:DescribeTable",
                        "dynamodb:Get*",
                        "dynamodb:PutItem",
                        "dynamodb:Query",
                        "dynamodb:Scan",
                        "dynamodb:UpdateItem"
                     ],
            "Resource": "${aws_dynamodb_table.personapi_dynamodb_table.arn}"
        }
    ]
}
EOF
}
