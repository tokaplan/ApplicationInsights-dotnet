using IntegrationTests.WebApp;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
{
    /// <summary>
    /// This is a partial class.
    /// This file is encapsulating all the framework preprocessors so we can keep the actual test files clean.
    /// </summary>
#if NET5_0
    public partial class RequestCollectionTest : IClassFixture<CustomWebApplicationFactory<Startup_net_5_0>>
#elif NETCOREAPP3_1
    public partial class RequestCollectionTest : IClassFixture<CustomWebApplicationFactory<Startup_netcoreapp_3_1>>
#else
    public partial class RequestCollectionTest :  IClassFixture<CustomWebApplicationFactory<Startup_netcoreapp_2_1>>
#endif
    {

#if NET5_0
        internal readonly CustomWebApplicationFactory<Startup_net_5_0> _factory;
#elif NETCOREAPP3_1
        internal readonly CustomWebApplicationFactory<Startup_netcoreapp_3_1> _factory;
#else
        internal readonly CustomWebApplicationFactory<Startup_netcoreapp_2_1> _factory;
#endif

        protected readonly ITestOutputHelper output;

#if NET5_0
        public RequestCollectionTest(CustomWebApplicationFactory<Startup_net_5_0> factory, ITestOutputHelper output)
#elif NETCOREAPP3_1
        public RequestCollectionTest(CustomWebApplicationFactory<Startup_netcoreapp_3_1> factory, ITestOutputHelper output)
#else
        public RequestCollectionTest(CustomWebApplicationFactory<Startup_netcoreapp_2_1> factory, ITestOutputHelper output)
#endif
        {
            this.output = output;
            _factory = factory;
            _factory.sentItems.Clear();
        }
    }
}
