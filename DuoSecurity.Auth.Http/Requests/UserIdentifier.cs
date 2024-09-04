using System;
using System.Collections.Generic;

namespace DuoSecurity.Auth.Http.Requests;

public struct UserIdentifier
{
    public static UserIdentifier UserId(string value)
        => new(UserIdentifierType.UserId, value);
    
    public static UserIdentifier UserName(string value)
        => new(UserIdentifierType.UserName, value);
    
    public UserIdentifierType Type { get; }
    
    public string Value { get; }

    private UserIdentifier(UserIdentifierType type, string value)
    {
        Type = type;
        Value = value;
    }

    public KeyValuePair<string, string> GetParameter()
        => Type switch
        {
            UserIdentifierType.UserId => new KeyValuePair<string, string>("user_id", Value),
            UserIdentifierType.UserName => new KeyValuePair<string, string>("username", Value),
            _ => throw new NotSupportedException("Unsupported user identifier type."),
        };
}