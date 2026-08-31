using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.Client.Abstract;

/// <summary>
/// Provides an authenticated, cached <see cref="HttpClient"/> for Telnyx's REST API.
/// </summary>
public interface ITelnyxHttpClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Removes and disposes the HTTP client owned by this provider.
    /// </summary>
    new void Dispose();

    /// <summary>
    /// Asynchronously removes and disposes the HTTP client owned by this provider.
    /// </summary>
    new ValueTask DisposeAsync();

    /// <summary>
    /// Gets the cached Telnyx HTTP client, creating it on first use.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
