using System;
using System.Collections.Generic;
using Amazon.CDK.AWS.IAM;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Security.Roles.ECR
{
    public class ECRAccess
    {
        public ECRAccess()
        {
        }

        public PolicyStatement[] Statements
        {
            get
            {
                return new PolicyStatement[] {
                    GenerateECRStatements(),
                    GenerateKMSStatments(),
                    AllowS3Statments(),
                    AllowLogStatments()
                };
            }
        }

        private PolicyStatement GenerateECRStatements()
        {
            var statementProps = new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[] {
                "imagebuilder:GetComponent",
                "imagebuilder:GetContainerRecipe",
                "ecr:GetAuthorizationToken",
                "ecr:BatchGetImage",
                "ecr:InitiateLayerUpload",
                "ecr:UploadLayerPart",
                "ecr:CompleteLayerUpload",
                "ecr:BatchCheckLayerAvailability",
                "ecr:GetDownloadUrlForLayer",
                "ecr:PutImage"
                },
                Resources = new string[] { "*" }
            };

            var statement = new PolicyStatement(statementProps);

            return statement;

        }

        private PolicyStatement GenerateKMSStatments()
        {

            var condtions = new Dictionary<string, object>();

            condtions.Add("ForAnyValue:StringEquals",
                    "{"
                    + "\"kms:EncryptionContextKeys\": \"aws:imagebuilder:arn\","
                    + "\"aws:CalledVia\": [ "
                        + "\"imagebuilder.amazonaws.com\" "
                    + " ]"
                    + "}"


             );

            var statementProps = new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[]
                {
                    "kms:Decrypt"
                },
                Resources = new string[] { "*" },
                //Conditions = condtions
            };

            var statement = new PolicyStatement(statementProps);

            return statement;

        }

        private PolicyStatement AllowS3Statments()
        {
            var statementProps = new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[]
                {
                    "s3:GetObject"
                },
                Resources = new string[] { "arn:aws:s3:::ec2imagebuilder*" }
            };

            var statement = new PolicyStatement(statementProps);

            return statement;

        }

        private PolicyStatement AllowLogStatments()
        {
            var statementProps = new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[]
                {
                    "logs:CreateLogStream",
                    "logs:CreateLogGroup",
                    "logs:PutLogEvents"
                },
                Resources = new string[] { "arn:aws:logs:*:*:log-group:/aws/imagebuilder/*" }
            };

            var statement = new PolicyStatement(statementProps);

            return statement;

        }
    }
}
