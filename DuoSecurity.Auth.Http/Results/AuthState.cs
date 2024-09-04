namespace DuoSecurity.Auth.Http.Results;

public enum AuthState
{
    /// <summary>
    /// Your application should grant access to the user.
    /// </summary>
    Allow,
    /// <summary>
    /// Your application should not grant access to the user.
    /// </summary>
    Deny
}