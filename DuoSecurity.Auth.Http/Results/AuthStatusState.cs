﻿using System.Text.Json.Serialization;
using DuoSecurity.Auth.Http.Internal;

namespace DuoSecurity.Auth.Http.Results;

[JsonConverter(typeof(UnderscoreSnakeCasedConverter))]
public enum AuthStatusState
{
    /// <summary>
    /// Authentication succeeded. Your application should grant access to the user.
    /// </summary>
    Allow,
    /// <summary>
    /// Authentication denied. Your application should deny access.
    /// </summary>
    Deny,
    /// <summary>
    /// Authentication is still in-progress. Your application should poll again until it finishes. Check the status for more details on the progress.
    /// </summary>
    Waiting
}