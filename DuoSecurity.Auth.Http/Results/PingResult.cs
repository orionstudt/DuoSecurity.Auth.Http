using DuoSecurity.Auth.Http.JsonModels;
using System;

namespace DuoSecurity.Auth.Http.Results;

public class PingResult
{
    /// <summary>
    /// Current server time.
    /// </summary>
    public DateTime TimeUtc { get; set; }

    internal PingResult(PingResultModel model)
    {
            // UNIX Timestamp to UTC time
            TimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(model.Time);
        }
}