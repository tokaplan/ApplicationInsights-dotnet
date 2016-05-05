﻿using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SampleWebAppIntegration.FunctionalTest
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using FunctionalTestUtils;
    using Microsoft.ApplicationInsights.DataContracts;

    public class TelemetryModuleWorkingWebApiTests : TelemetryTestsBase
    {
        private const string assemblyName = "WebApiShimFw46.FunctionalTests";

#if NET451

        [Fact]
        public void TestBasicDependencyPropertiesAfterRequestingBasicPage()
        {
            using (var server = new InProcessServer(assemblyName))
            {
                const string RequestPath = "/api/values";

                var expectedDependencyTelemetry = new DependencyTelemetry();
                expectedDependencyTelemetry.Name = server.BaseHost + RequestPath;
                expectedDependencyTelemetry.ResultCode = "200";
                expectedDependencyTelemetry.Success = true;
                this.ValidateBasicDependency(server, RequestPath, expectedDependencyTelemetry);
            }
        }

        [Fact]
        public void TestIfPerformanceCountersAreCollected()
        {
            using (var server = new InProcessServer(assemblyName))
            {
                const string RequestPath = "/";
                var httpClient = new HttpClient();
                var task = httpClient.GetAsync(server.BaseHost + RequestPath);
                task.Wait(TestTimeoutMs);
                var result = task.Result;
                Thread.Sleep(70000);
                var actual = server.BackChannel.Buffer.OfType<PerformanceCounterTelemetry>().Distinct();

                Assert.True(actual.Count() > 0);

            }
        }

#endif
    }
}
