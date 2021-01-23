﻿using System;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;

namespace GeekCafe.AWSCDK.DevOps.Stacks.DataStorage.Databases.RDSDatabases
{
    public class MySQLStack: Stack
    {
        public DatabaseInstance DBInstance { get; private set; }
        public MySQLStack(Construct scope, string id, string project, string environment, IStackProps props = null) : base(scope, $"{id}", props)
        {
        }

        public DatabaseInstance Create(Vpc vpc, SecurityGroup sg, string id)
        {
            var db = new DatabaseInstance(this, "RDSMySQLDB", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Mysql(new MySqlInstanceEngineProps
                {
                    Version = MysqlEngineVersion.VER_8_0,
                }),
                // todo get credital types later
                // create a username and password
                Credentials = Credentials.FromPassword("master", new SecretValue("sup3rs3cr3t")),

                //create a user and password stored in SSM
                //Credentials = Credentials.FromPassword("master", SecretValue.SsmSecure("dev/db/password", "1")),
                //this will create a new user named "master" and generate a password and store it in the the secrets manager
                //Credentials = Credentials.FromGeneratedSecret("master"),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.SMALL),
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.ISOLATED
                },
                Vpc = vpc,
                MultiAz = true,
                BackupRetention = Duration.Days(7),
                StorageEncrypted = true,
                AutoMinorVersionUpgrade = true,
                StorageType = StorageType.GP2,
                SecurityGroups = new[] { BuildMySQLSG(this, vpc, sg, "rds-mysql-access") },
                InstanceIdentifier = id,
                DeletionProtection = true,

            });


            // rotate the master password (use this when storing it in secrets manager)
            //db.AddRotationSingleUser();

            //EaSdRDpAgGjGKd0AL-uI2fwSJ,znW5

            DBInstance = db;

            return db;
        }


        private SecurityGroup BuildMySQLSG(Construct scope, Vpc vpc, SecurityGroup sourceSecurityGroup, string name = null)
        {
            var props = new SecurityGroupProps
            {
                AllowAllOutbound = true,
                Vpc = vpc,
                Description = name,
                SecurityGroupName = $"sgroup-{name}",
            };

            // create some security groups
            var sg = new SecurityGroup(scope, $"sgroup-{name}", props);


            var peer = (sourceSecurityGroup != null) ? sourceSecurityGroup : GenerateSourceSecurityGroup(scope, vpc);

            var port = new Port(GetPortProps(3306, 3306));


            sg.AddIngressRule(peer, port, name);




            return sg;
        }

        private SecurityGroup GenerateSourceSecurityGroup(Construct scope, Vpc vpc)
        {
            var props = new SecurityGroupProps
            {
                AllowAllOutbound = true,
                Vpc = vpc,
                Description = "RDS Access",
                SecurityGroupName = $"sgroup-rds-access",
            };

            // create some security groups
            var sg = new SecurityGroup(scope, $"sgroup-rds-access", props);

            return sg;
        }

        private PortProps GetPortProps(int from, int to)
        {
            var portProps = new PortProps
            {
                FromPort = from,
                ToPort = to,
                Protocol = Protocol.TCP,
                StringRepresentation = "??"
            };

            return portProps;
        }
    }
}
