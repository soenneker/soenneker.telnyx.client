using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Telnyx.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class TelnyxClientUtilTests : HostedUnitTest
{
    private readonly ITelnyxHttpClient _util;

    public TelnyxClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<ITelnyxHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
