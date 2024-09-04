using System.Text.Json;
using DuoSecurity.Auth.Http.Results;
using FluentAssertions;
using NUnit.Framework;

namespace DuoSecurity.Auth.Http.Tests;

[TestFixture]
public class SerializationTests
{
    [TestCase("allow", AuthState.Allow)]
    [TestCase("deny", AuthState.Deny)]
    public void AuthState_can_deserialize(string value, AuthState expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<AuthState>(json);
        actual.Should().Be(expected);
    }

    [TestCase("calling", AuthStatus.Calling)]
    [TestCase("answered", AuthStatus.Answered)]
    [TestCase("pushed", AuthStatus.Pushed)]
    [TestCase("push_failed", AuthStatus.PushFailed)]
    [TestCase("timeout", AuthStatus.Timeout)]
    [TestCase("fraud", AuthStatus.Fraud)]
    [TestCase("allow", AuthStatus.Allow)]
    [TestCase("bypass", AuthStatus.Bypass)]
    [TestCase("deny", AuthStatus.Deny)]
    [TestCase("locked_out", AuthStatus.LockedOut)]
    [TestCase("sent", AuthStatus.Sent)]
    public void AuthStatus_can_deserialize(string value, AuthStatus expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<AuthStatus>(json);
        actual.Should().Be(expected);
    }

    [TestCase("allow", AuthStatusState.Allow)]
    [TestCase("deny", AuthStatusState.Deny)]
    [TestCase("waiting", AuthStatusState.Waiting)]
    public void AuthStatusState_can_deserialize(string value, AuthStatusState expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<AuthStatusState>(json);
        actual.Should().Be(expected);
    }

    [TestCase("phone", DeviceType.Phone)]
    [TestCase("token", DeviceType.Token)]
    public void DeviceType_can_deserialize(string value, DeviceType expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<DeviceType>(json);
        actual.Should().Be(expected);
    }

    [TestCase("auto", DeviceCapability.Auto)]
    [TestCase("push", DeviceCapability.Push)]
    [TestCase("sms", DeviceCapability.Sms)]
    [TestCase("phone", DeviceCapability.Phone)]
    [TestCase("mobile_otp", DeviceCapability.MobileOtp)]
    public void DeviceCapability_can_deserialize(string value, DeviceCapability expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<DeviceCapability>(json);
        actual.Should().Be(expected);
    }
    
    [TestCase("success", EnrollStatus.Success)]
    [TestCase("invalid", EnrollStatus.Invalid)]
    [TestCase("waiting", EnrollStatus.Waiting)]
    public void EnrollStatus_can_deserialize(string value, EnrollStatus expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<EnrollStatus>(json);
        actual.Should().Be(expected);
    }

    [TestCase("auth", PreAuthState.Auth)]
    [TestCase("allow", PreAuthState.Allow)]
    [TestCase("deny", PreAuthState.Deny)]
    [TestCase("enroll", PreAuthState.Enroll)]
    public void PreAuthState_can_deserialize(string value, PreAuthState expected)
    {
        var json = $"\"{value}\"";
        var actual = JsonSerializer.Deserialize<PreAuthState>(json);
        actual.Should().Be(expected);
    }
}