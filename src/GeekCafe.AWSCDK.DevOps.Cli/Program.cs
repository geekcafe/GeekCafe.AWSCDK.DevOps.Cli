using System;
using Amazon.CDK;

namespace GeekCafe.AWSCDK.DevOps.Cli
{
    sealed class Program
    {
        public static void Main(string[] args)
        { 

            var app = new App();

            var environment = "beta";
            var project = $"cdk-cli";
            var id = $"{environment}-{project}";
            // get the stack name from the args or a setting
            //var name = $"cdk-proving-ground";
            //name = "WebAppStack";
            LogArgs(args);

            // are we creating a vpc

            var vpcStack = new Stacks.VpcStack(app, id, project, environment);

            ApplyDefaultTags(vpcStack, environment, project);

            var createDB = true;

            if(createDB)
            {
                var list = new string[] { "", "-dev", "-qa"};
                foreach (var db in list)
                {
                    var dbInstance = new Stacks.DataStorage.Databases.RDSDatabases.MySQLStack(app, $"id-mysql-stack{db}", project, environment);
                    dbInstance.Create(vpcStack.Vpc, null, id);
                }
                
            }
            

            var cloudAssembly = app.Synth();
        }

        private static void ApplyDefaultTags(IConstruct construct, string environment, string project)
        {
            Tags.Of(construct).Add("Stack-Author", "Eric Wilson");
            Tags.Of(construct).Add("Stack-Version", "0.0.1-beta");
            Tags.Of(construct).Add("Stack-Tool", "GeekCafe-AWS-CDK");
            Tags.Of(construct).Add("Stack-Environment", environment);
            Tags.Of(construct).Add("Stack-Project", project);

        }

        public static void LogArgs(string[] args)
        {
            try
            {
                // writing a log file for the args
                var filePath = System.IO.Directory.GetCurrentDirectory();
                filePath = System.IO.Path.Join(filePath, "logs");
                System.IO.Directory.CreateDirectory(filePath);

                filePath = System.IO.Path.Join(filePath, $"args-{System.DateTime.UtcNow.Ticks.ToString()}.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    file.WriteLine("The following Args were found:");
                    foreach (string arg in args)
                    {
                        file.WriteLine($"arg: {arg}");
                    }

                    file.WriteLine("-- end of args");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception: {ex.Message}");
            }
        }

    }
}
