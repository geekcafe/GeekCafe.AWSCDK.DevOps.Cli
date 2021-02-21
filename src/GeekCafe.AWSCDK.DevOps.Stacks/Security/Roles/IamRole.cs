using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks.Security.Roles
{
    public class IamRole
    {
        public IamRole()
        {
        }

        public Role Create(Construct scope, string name)
        {


            var props = new RoleProps
            {
                RoleName = name,
                ManagedPolicies = GetManagedPolicies(scope),
                AssumedBy = new ServicePrincipal("ec2.amazonaws.com")

            };
            var role = new Role(scope, "iam-role", props);

            return role;
        }

        public ManagedPolicy[] GetManagedPolicies(Construct scope)
        {

            var ssmStatements = new Security.Roles.SSM.ManagedInstanceCore().Statements;
            var ssmAssume = new Security.Roles.SSM.TrustedCommunication().Statements;
            var ec2Statements = new Security.Roles.EC2.FullAccess().Statements;
            var s3Statments = new Security.Roles.S3.FullAccess().Statements;
            var ecrStatements = new Security.Roles.ECR.ECRAccess().Statements;
            var policies = new List<ManagedPolicy>();

            policies.Add(Policy(scope, "ssm-statements", null, ssmStatements));
            //policies.Add(Policy(scope, "ssm-assume", null, ssmAssume));
            policies.Add(Policy(scope, "ec2-full-access", null, ec2Statements));
            policies.Add(Policy(scope, "s3-full-access", null, s3Statments));
            policies.Add(Policy(scope, "ecr-access", null, ecrStatements));



            return policies.ToArray();
        }

        public ManagedPolicy Policy(Construct scope, string id, string policyName, PolicyStatement[] statements)
        {

            if (policyName == null) policyName = id;

            var props = new ManagedPolicyProps
            {
                ManagedPolicyName = policyName,
                Statements = statements


            };
            var policy = new ManagedPolicy(scope, id, props);

            return policy;
        }
    }
}
