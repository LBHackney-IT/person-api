data "aws_iam_policy_document" "app_config_retrieval_assume_role" {
  statement {
    actions = [
      "sts:AssumeRole"
    ]
    principals {
      identifiers = ["ssm.amazonaws.com"]
      type        = "Service"
    }
  }
}

resource "aws_iam_role" "app_config_retrieval_role" {
  name               = "LBH_AppConfig_Get_Parameter_Store_Values"
  tags               = local.default_tags
  assume_role_policy = data.aws_iam_policy_document.app_config_retrieval_assume_role.json
}

data "aws_iam_policy_document" "app_config_retrieval_policy" {
  statement {
    actions = [
      "ssm:GetParameters",
      "ssm:GetParameter"
    ]
    effect = "Allow"
    resources = [
      "*"
    ]
  }
}

resource "aws_iam_policy" "app_config_retrieval" {
  tags   = local.default_tags
  policy = data.aws_iam_policy_document.app_config_retrieval_policy.json
}

resource "aws_iam_role_policy_attachment" "app_config_retrieval" {
  role       = aws_iam_role.app_config_retrieval_role.name
  policy_arn = aws_iam_policy.app_config_retrieval.arn
}
