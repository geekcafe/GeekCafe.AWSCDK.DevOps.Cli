using System;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using GeekCafe.AWSCDK.DevOps.Configuration;

namespace GeekCafe.AWSCDK.DevOps.Stacks.DataStorage.Databases.RDSDatabases
{
    public class MySQLStack: Stack
    {
        public DatabaseInstance DBInstance { get; private set; }        
        
        public MySQLStack(Construct scope, string stackName, IStackProps props = null) : base(scope, $"{stackName}", props)
        {
            
        }

        public DatabaseInstance Create(Amazon.CDK.AWS.EC2.Vpc vpc, IConfigSettings configSettings, SecurityGroup[] securityGroups)
        {
           
            var db = new DatabaseInstance(this, $"{configSettings.Rds.Name}", new DatabaseInstanceProps
            {
                // todo change all properties based on config settings
                Engine = DatabaseInstanceEngine.Mysql(new MySqlInstanceEngineProps
                {
                    //todo change based on config settings
                    Version = MysqlEngineVersion.VER_5_7,
                }),
               
                Credentials = GetCredentials(configSettings),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.SMALL),
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.ISOLATED
                },
                Vpc = vpc,
                MultiAz = configSettings.Rds.MultiAz,
                BackupRetention = Duration.Days(configSettings.Rds.BackupRetentionInDays),
                StorageEncrypted = configSettings.Rds.StorageEncrypted,
                AutoMinorVersionUpgrade = configSettings.Rds.AutoMinorVersionUpgrade,
                // todo
                StorageType = StorageType.GP2,
                SecurityGroups = securityGroups,
                InstanceIdentifier = configSettings.Rds.Name,
                DeletionProtection = configSettings.Rds.DeletionProtection,

            });


            // rotate the master password (use this when storing it in secrets manager)
            //db.AddRotationSingleUser();

            //EaSdRDpAgGjGKd0AL-uI2fwSJ,znW5

            DBInstance = db;

            return db;
        }

        /// <summary>
        /// For now just return it.
        /// TODO store this in the parameter store and only access it that way
        /// </summary>
        /// <param name="configSettings"></param>
        /// <returns></returns>
        public string GetConnectionString(IConfigSettings configSettings)
        {
            var host = DBInstance.DbInstanceEndpointAddress;
            var port = DBInstance.DbInstanceEndpointPort;
            var userName = configSettings.Rds.UserName;
            var password = configSettings.Rds.Password;

            var connString = $"Server={host};Port={port};Database=;User={userName};Password={password}";

            return connString;
        }

        private Credentials GetCredentials(IConfigSettings configSettings)
        {
            // todo get credital types later
            // create a username and password

            Core.Logging.Logger.Log($"RDS: UserName: {configSettings.Rds.UserName}", Microsoft.Extensions.Logging.LogLevel.Debug);
            Core.Logging.Logger.Log($"RDS: Password: {configSettings.Rds.Password}", Microsoft.Extensions.Logging.LogLevel.Debug);

            var credentials = Credentials.FromPassword(configSettings.Rds.UserName
                , new SecretValue(configSettings.Rds.Password));

            
            //create a user and password stored in SSM
            //Credentials = Credentials.FromPassword("master", SecretValue.SsmSecure("dev/db/password", "1")),
            //this will create a new user named "master" and generate a password and store it in the the secrets manager
            //Credentials = Credentials.FromGeneratedSecret("master"),
            return credentials;
        }

        

        
        
    }
}
