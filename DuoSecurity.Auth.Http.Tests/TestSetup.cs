using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DuoSecurity.Auth.Http.Tests;

[SetUpFixture]
public static class TestSetup
{
    public static DuoAuthClient Client { get; private set; } = null!;

    public static string UserName { get; private set; } = null!;
    
    [OneTimeSetUp]
    public static void OneTimeSetUp()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(TestSetup).Assembly)
            .Build().Get<TestConfig>()!;

        var duoConfig = new DuoAuthConfig(
            configuration.HostName,
            configuration.IntegrationKey,
            configuration.SecretKey);

        Client = new DuoAuthClient(duoConfig);
        UserName = configuration.UserName;
    }

    [OneTimeTearDown]
    public static void OneTimeTearDown()
    {
        Client?.Dispose();
    }

    private record TestConfig(string IntegrationKey, string SecretKey, string HostName, string UserName);
}