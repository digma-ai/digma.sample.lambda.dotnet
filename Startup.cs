using AspNetOnLambda.Services;
using OpenTelemetry.AutoInstrumentation.Digma;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AspNetOnLambda;

public class Startup
{
    private static readonly DateTime Timestamp = DateTime.UtcNow;
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRandomNumbersService, RandomNumbersService>();
        services.AddControllers();
        //Use Digma extended observability
        new Plugin().SyncInitializing();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync($"Welcome to running ASP.NET Core on AWS Lambda {Timestamp:u}");
            });
        });
    }
}