# Welcome to your CDK C# project!

You should explore the contents of this project. It demonstrates a CDK app with an instance of a stack (`SrcStack`)
which contains an Amazon SQS queue that is subscribed to an Amazon SNS topic.

The `cdk.json` file tells the CDK Toolkit how to execute your app.

It uses the [.NET Core CLI](https://docs.microsoft.com/dotnet/articles/core/) to compile and execute your project.

## Useful commands

* `npm install -g aws-cdk`
* `npm upgrade -g aws-cdk`
* `dotnet build src` compile this app
* `cdk ls`           list all stacks in the app
* `cdk synth`       emits the synthesized CloudFormation template
* `cdk deploy`      deploy this stack to your default AWS account/region
* `cdk diff`        compare deployed stack with current state
* `cdk docs`        open CDK documentation

## Passing Parameters
* `cdk synth -a "dotnet run -p src/GeekCafe.AWSCDK.DevOps.Cli/GeekCafe.AWSCDK.DevOps.Cli.csproj -one -two three"`
* `cdk deploy --profile geekcafe -a "dotnet run -p src/GeekCafe.AWSCDK.DevOps.Cli/GeekCafe.AWSCDK.DevOps.Cli.csproj -one -two three"`
* `cdk deploy --profile geekcafe` specify a profile
* `cdk deploy --profile geekcafe beta-cdk-cli-vpc-stack` specify a specific stack (when multiple stacks are in the cli)


## Other examples of passing in args
* `cdk synth -a "dotnet GeekCafe.AWSCDK.DevOps.Cli.dll -e qa -p geek"`
* `cdk synth -a "dotnet GeekCafe.AWSCDK.DevOps.Cli.dll -e qa -p geek" qa-geek-vpc-stack`


Enjoy!
