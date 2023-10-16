# Litmus Interview Project Template


 ## Building
 We use use [CAKE](https://cakebuild.net/) at Litmus to keep our build process in source control and make this process available on our local machine.

To build this solution, you can run `dotnet cake` on the root directory of this project.

## Testing
We use [NUnit](https://nunit.org/) and [NSubsitute](https://nsubstitute.github.io/) as our testing framework.  Our unit-tests tend to follow the Behavior-Driven Development (BDD) pattern, so you will see Given/When/Then in the samples we have provided to you.

## Common Patterns / Services
Our team has a collection of services and patterns which encouraging team best practices.  They also help us deliver and support our applications.  We have imported in some lightweight versions of these to give you a feel for what it is like to work in our code base and to save you from writing common boiler plate code.

### Logging
The Infrastructure team has dozens of applications we support deployed across thousands of instances.  In order to keep application logging centeralized and accessible we use [Serilog](https://github.com/serilog/serilog) to send structured logs to [Seq](https://datalust.co/seq).  In this project we have provided the same `IStructuredLogger` interface we use, but instead write those log messages to the Console.  Please use this logger in your project.

### Depenency Injection
Given the size of our code base and the number of applications we support, we have found Dependency Injection (DI) an important tool.  We use [Unity](https://github.com/unitycontainer/unity) with an abstraction layer which makes it easier to consume.  We also have tooling to help prevent some common errors we have run into.

### Services via Topshelf
Creating Windows Services is a pain.  [Topshelf](http://topshelf-project.com/) makes this process painless and also supports our efforts to migrate services to Linux. We have wrapped our best practices and DI code into `ServiceHostFactory`

### Web API 
Many of the services we build are consumed by our Applications Team in Ruby.  In order to make these services discoverable and accessible we publish a [Swagger UI](https://swagger.io/tools/swagger-ui/) for all our APIs. `Litmus.Core.AspNetCore` provides our standard Litmus branded UI and helpers to plumb in our DI system.  We have a few helper methods in `Program.cs` and `Startup.cs` 

**Program.cs**
```
public static void Main(string[] args) => 
    CreateHostBuilder(args).Build().Run();
            
 public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        // Wire up the dependency module 
        .UseDependencyModuleServiceProvider<##YourDependencyModule##>()
        .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
```

**Startup.cs**
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // Add litmus branded swagger services
    services.AddLitmusSwagger();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Setup the routes for Swagger to work correctly
    app.UseLitmusSwagger();

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();

        // Redirect / to swagger
        endpoints.Map("/", context =>
        {
            context.Response.Redirect("swagger/index.html");
            return Task.CompletedTask;
        });
    });
}
```

## Sample Applications
In this solution you will find two sample applications `Litmus.Core.Sample.AspNetCore` and `Litmus.Core.Sample.ConsoleApplication`.  They demonstrate how to use the code described above.  
