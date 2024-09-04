namespace DuoSecurity.Auth.Http.Results;

public sealed record EnrollStatusResult
{
    /// <summary>
    /// Indicates whether a user has completed enrollment.
    /// </summary>
    public required EnrollStatus Status { get; init; }
}