using ETLAthena.Core.Services;
using ETLAthena.Core.Services.Validation;
using ETLAthena.Core.Services.Merging;
using ETLAthena.Core.Services.Transformation;
using ETLAthena.Core.Models;
using ETLAthena.Core.DataStorage;
using Serilog;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;

namespace ETLAthena.API;
public class Program
{
    public static void Main(string[] args)
    {
        string logFilePath = "C:/Users/rober/OneDrive/Documents/_Personal/Code/SavillsRecruitment/tech-test-be-athena-robert-ruta/logs/temp.log";
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Infinite)
            .CreateLogger();
        
        try
        {
            Log.Information("Starting web host!");
            CreateHostBuilder(args).Build().Run();            
            Log.Information("Web host has closed!");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    public IConfiguration Configuration { get; }


    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // ===== DEPENDENCY INJECTION ===== //

        services.AddScoped<IDataIngestionService, DataIngestionService>();
        services.AddScoped<IDataProcessingService, DataProcessingService>();
        services.AddSingleton<IDataStorageService, InMemoryDataStore>();
        
        // Register validators
        services.AddScoped<IS1Validator, S1Validator>();
        services.AddScoped<IS2Validator, S2Validator>();
        
        // Register transformers
        services.AddScoped<IS1Transformer, S1Transformer>();
        services.AddScoped<IS2Transformer, S2Transformer>();

        // Register merger
        services.AddScoped<IMerger, Merger>();

        // Bind ApplicationSettings
        var appSettings = new ApplicationSettings();
        Configuration.GetSection("ApplicationSettings").Bind(appSettings);

        // Register ApplicationSettings
        services.AddSingleton(appSettings);

        // Register AWS S3 client
        services.AddAWSService<IAmazonS3>();

        services.AddHttpClient();
        services.AddScoped<IDataPullService, DataPullService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataStorageService dataStorageService, IDataIngestionService dataIngestionService, IS1Transformer s1Transformer)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map API controllers
        });
        
        LoadInitialS1Data(dataStorageService, dataIngestionService, s1Transformer);
    }

    private void LoadInitialS1Data(IDataStorageService dataStorageService, IDataIngestionService dataIngestionService, IS1Transformer s1Transformer)
    {
        var appSettings = new ApplicationSettings();
        Configuration.GetSection("ApplicationSettings").Bind(appSettings);
        string jsonPath = appSettings.LoadDataPath;

        string s1Json = File.ReadAllText(jsonPath);

        List<S1Model> s1Data = dataIngestionService.IngestBulkDataFromSourceS1(s1Json);
        List<BuildingModel> buildings = s1Transformer.Transform(s1Data);
        dataStorageService.UpdateOrCreateBuildings(buildings);
    }
}
