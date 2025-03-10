﻿using Soenneker.Telnyx.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Telnyx.Client.Tests;

[Collection("Collection")]
public class TelnyxClientUtilTests : FixturedUnitTest
{
    private readonly ITelnyxClientUtil _util;

    public TelnyxClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ITelnyxClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
