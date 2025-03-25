# Digma OTEL Instrumentation with .NET API Serverless Application

This project is based on a standard .NET lambda example posted [here](https://github.com/IvanFarkas/serverless_image_AspNetCoreWebAPI) which has been modified to use OpenTelemetry using the AWS OTEL Distro layer (with no code changes). It also uses the Digma auto-instrumentation to add tracing for the application internal code.

### SAML Template ###

You can find the template for deploying the lambda in the [deploy](/deloy) folder. 

#### Environment variables ####

The template references the OTEL Distro template and includes the following environment variables:

| Key      | Value | Description |
| -------- | ------- | ------- |
| OTEL_EXPORTER_OTLP_ENDPOINT | None | Replace with the Digma collector URL |
| OTEL_EXPORTER_OTLP_PROTOCOL | grpc | The protocol to use (grpc or http)   |
| OTEL_SERVICE_NAME   | AspNetOnLambda | The name of this lambda application    |
| OTEL_DOTNET_AUTO_NAMESPACES | AspNetOnLambda.Services
 | Namespaces which you wish to be automatically instrumented by Digma (comma separated) |
| OTEL_RESOURCE_ATTRIBUTES | digma.environment=DIGMA_ENV,digma.environment.type=Public
 | Use this env variable to inject two mandatory observability attributes for Digma: the environment name and type |
| OTEL_DOTNET_AUTO_TRACES_ADDITIONAL_SOURCES | * | Controls sources that will be included in tracing, can keep defualt at '*' |
| AWS_LAMBDA_EXEC_WRAPPER | /opt/otel-instrument
| Keep the default value for auto instrumentation
| OTEL_TRACES_EXPORTER | otlp | Keep the default value of 'otlp' |
      

#### Activating Tracing ####

To enable tracing, the Lambda resource also have the following attribute: `Tracing: Active`. 

This can configurated in the AWS Console for the lambda under `Configuration` -> `Monitoring and operations tools` -> `Edit` -> `CloudWatch Application Signals and AWS X-Ray` -> `Lambda service trace` -> `Enable`



### Autoinstrumenting the code ###

This project uses the [OpenTel.AutoInstrumentation.Digma](https://github.com/digma-ai/OpenTelemetry.Instrumentation.Digma/tree/main/src/OpenTelemetry.AutoInstrumentation.Digma) package. This means we can add instrumentation to function in the namespace. This is normally activated via an environment variable, however for lambda functions we need to initial the autoinstrumentation on startup. This is done in the `Startup.cs` file:

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRandomNumbersService, RandomNumbersService>();
        services.AddControllers();
        //Use Digma extended observability
        new Plugin().SyncInitializing();
    }
```

### Output in Digma ###

Once you've set the right URL and environment settings you'll be able to see the entire trace and assetes show up in your Digma install.



## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Deploy application
```
    cd "AspNetOnLambda/src/AspNetOnLambda"
    dotnet lambda deploy-serverless
```
