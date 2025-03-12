using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;

namespace AlzaBoxApiTests;

public class TestBase
{
    protected IConfiguration Configuration { get; private set; }
    protected ILogger Logger { get; private set; }

    [OneTimeSetUp]
    public void Setup()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
        Logger = loggerFactory.CreateLogger<TestBase>();
    }
}
