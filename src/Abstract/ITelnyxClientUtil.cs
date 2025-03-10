using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Telnyx.Client.Abstract;

/// <summary>
/// A .NET thread-safe singleton HttpClient for Telnyx
/// </summary>
public interface ITelnyxClientUtil : IDisposable, IAsyncDisposable
{
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
