Imports System.IO
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Serilog
Module Program
    Sub Main(args As String())
        Dim builder As IConfigurationBuilder = New ConfigurationBuilder()
        builder.SetBasePath(Directory.GetCurrentDirectory).
        AddJsonFile("appsettings.json", optional:=False, reloadOnChange:=True).
        AddJsonFile($"appsetting.{If(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production")}.json", [optional]:=True).
        AddEnvironmentVariables()

        Dim LoggerConfiguration = New LoggerConfiguration().ReadFrom.
        Configuration(builder.Build()).
        Enrich.FromLogContext()

        Dim AspNetHost = Host.CreateDefaultBuilder().
        ConfigureServices(Sub(X, Y)
                              ConfigureDI(X, Y)
                          End Sub).
        UseSerilog().
        Build()

        LoggerConfiguration.WriteTo.File("test.txt").WriteTo.Console()
        Log.Logger = LoggerConfiguration.CreateLogger()
        Log.Logger.Information("start")

        Dim Test As IWriteToPostgres = AspNetHost.Services.GetService(Of WriteToPostgres)
        Test.Write(1000)

    End Sub
    Sub ConfigureDI(Context As HostBuilderContext, ByRef Services As IServiceCollection)
        Services.AddScoped(Of WriteToPostgres)
        Services.AddScoped(Of WeatherForecast)
        Services.AddDbContext(Of ApplicationDbContext)(Sub(options)
                                                           options.UseNpgsql(Context.Configuration.GetConnectionString("Default"))
                                                           options.EnableSensitiveDataLogging(True)
                                                           options.EnableDetailedErrors()
                                                       End Sub, ServiceLifetime.Transient)
    End Sub


End Module
