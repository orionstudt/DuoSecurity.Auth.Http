namespace DuoSecurity.Auth.Http.Results;

public class EnrollStatusResult
{
    /// <summary>
    /// Indicates whether a user has completed enrollment.
    /// </summary>
    public EnrollStatus Status { get; }

    internal EnrollStatusResult(string response)
    {
            switch (response?.ToLower())
            {
                case "success":
                    Status = EnrollStatus.Success;
                    break;
                case "waiting":
                    Status = EnrollStatus.Waiting;
                    break;
                default:
                    Status = EnrollStatus.Invalid;
                    break;
            }
        }
}