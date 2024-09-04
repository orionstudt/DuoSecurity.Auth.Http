using System;
using System.IO;

namespace DuoSecurity.Auth.Http.Results;

public sealed record LogoResult : IDisposable
{
    /// <summary>
    /// MIME Type of image.
    /// </summary>
    public string ContentType { get; } = "image/png";

    /// <summary>
    /// Logo data as a stream.
    /// </summary>
    public required Stream Stream { get; init; }

    public void Dispose()
    {
        Stream?.Dispose();
    }
}