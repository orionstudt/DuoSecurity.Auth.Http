using DuoSecurity.Auth.Http.Requests;
using DuoSecurity.Auth.Http.Results;
using FluentAssertions;
using NUnit.Framework;

namespace DuoSecurity.Auth.Http.Tests;

[TestFixture]
public class DuoAuthClientTests
{
    protected static DuoAuthClient Client => TestSetup.Client;

    protected static string UserName => TestSetup.UserName;

    [Test]
    public async Task Ping_works_as_expected()
    {
        var response = await Client.PingAsync(default);
        
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Error.Should().BeNull();
    }

    [Test]
    public async Task Check_works_as_expected()
    {
        var response = await Client.CheckAsync(default);
        
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Error.Should().BeNull();
    }

    [Test(Description = "Note that this test requires that your account has a logo configured.")]
    public async Task Logo_works_as_expected()
    {
        var response = await Client.LogoAsync(default);
        
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Error.Should().BeNull();
    }

    [Test(Description = "Note that this test will create an actual user on your account.")]
    public async Task Enroll_and_EnrollStatus_works_as_expected()
    {
        var first = await Client.EnrollAsync(new EnrollRequest
        {
            ValidSeconds = 30,
        }, default);
        
        first.Should().NotBeNull();
        first.IsSuccessful.Should().BeTrue();
        first.Result.Should().NotBeNull();
        first.Error.Should().BeNull();

        var enrollment = first.Result!;
        var userId = enrollment.UserId!;

        await Task.Delay(TimeSpan.FromSeconds(2));
        var second = await Client.EnrollStatusAsync(new EnrollStatusRequest
        {
            UserId = userId,
            ActivationCode = enrollment.ActivationCode,
        }, default);

        second.Should().NotBeNull();
        second.IsSuccessful.Should().BeTrue();
        second.Result.Should().NotBeNull();
        second.Error.Should().BeNull();

        second.Result!.Status.Should().Be(EnrollStatus.Waiting);
    }

    [Test(Description = "The given username should be one that is already enrolled.")]
    public async Task PreAuth_works_as_expected()
    {
        var response = await Client.PreAuthAsync(new PreAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(UserName),
        }, default);
        
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Error.Should().BeNull();

        var result = response.Result!;
        result.Devices.Should().NotBeEmpty();
    }

    [Test(Description = "The given username should be one that is already enrolled. This test will require auth to complete.")]
    public async Task Auth_works_as_expected()
    {
        var response = await Client.AuthAsync(new AutoAuthRequest
        {
            UserIdentifier = UserIdentifier.UserName(UserName),
        }, default);
        
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Error.Should().BeNull();
    }
}