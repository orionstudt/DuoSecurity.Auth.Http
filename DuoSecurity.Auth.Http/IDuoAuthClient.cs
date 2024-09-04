using System.Threading;
using System.Threading.Tasks;
using DuoSecurity.Auth.Http.Requests;
using DuoSecurity.Auth.Http.Results;

namespace DuoSecurity.Auth.Http;

/// <summary>
/// HTTP Client for interacting with the DuoSecurity Auth API.
/// </summary>
public interface IDuoAuthClient
{
    /// <summary>
    /// https://duo.com/docs/authapi#/ping
    /// </summary>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<PingResult>> PingAsync(CancellationToken cancel);
    
    /// <summary>
    /// https://duo.com/docs/authapi#/check
    /// </summary>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<PingResult>> CheckAsync(CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/logo
    /// </summary>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<LogoResult>> LogoAsync(CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/enroll
    /// </summary>
    /// <param name="request">The enrollment request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<EnrollResult>> EnrollAsync(EnrollRequest request, CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/enroll_status
    /// </summary>
    /// <param name="request">The enrollment status request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<EnrollStatusResult>> EnrollStatusAsync(EnrollStatusRequest request, CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/preauth
    /// </summary>
    /// <param name="request">The preauth request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<PreAuthResult>> PreAuthAsync(PreAuthRequest request, CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/auth
    /// </summary>
    /// <param name="request">The auth request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<AuthResult>> AuthAsync(AuthRequest request, CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/auth, but with async enabled
    /// </summary>
    /// <param name="request">The auth request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<AuthAsyncResult>> AuthWithPollingAsync(AuthRequest request, CancellationToken cancel);

    /// <summary>
    /// https://duo.com/docs/authapi#/auth_status
    /// </summary>
    /// <param name="request">The auth status request.</param>
    /// <param name="cancel">A cancellation token.</param>
    Task<DuoResponse<AuthStatusResult>> AuthStatusAsync(AuthStatusRequest request, CancellationToken cancel);
}