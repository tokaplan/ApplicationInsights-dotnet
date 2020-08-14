namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Tracing;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TestFramework;

    [TestClass]
    public class PortalDiagnosticsSenderTest
    {
        private readonly IList<ITelemetry> sendItems = new List<ITelemetry>();
        private readonly PortalDiagnosticsSender nonThrottlingPortalSender;

        private readonly PortalDiagnosticsSender throttlingPortalSender;

        private readonly IDiagnoisticsEventThrottlingManager throttleAllManager
            = new DiagnoisticsEventThrottlingManagerMock(true);

        private readonly IDiagnoisticsEventThrottlingManager dontThrottleManager
            = new DiagnoisticsEventThrottlingManagerMock(false);

        public PortalDiagnosticsSenderTest()
        {
            var configuration =
                new TelemetryConfiguration(Guid.NewGuid().ToString(), new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) });

            this.nonThrottlingPortalSender = new PortalDiagnosticsSender(
                configuration, 
                this.dontThrottleManager);

            this.throttlingPortalSender = new PortalDiagnosticsSender(
                configuration,
                this.throttleAllManager); 
        }

        [TestMethod]
        public void TestSendingOfEvent()
        {
            var evt = EventWrittenEventArgsMock.GetInstance(
                eventSourceName: "TelemetryCorrelation",
                level: EventLevel.Warning,
                message: "Error occurred at {0}, {1}",
                payload: new ReadOnlyCollection<object>(new List<object>() { "My function", "some failure" }));

            this.nonThrottlingPortalSender.Send(evt);

            Assert.AreEqual(1, this.sendItems.Count);
            var trace = this.sendItems[0] as TraceTelemetry;
            Assert.IsNotNull(trace);
            Assert.AreEqual(
                "AI (Internal): [TelemetryCorrelation] Error occurred at My function, some failure", 
                trace.Message);
        }

        [TestMethod]
        public void TestSendingWithSeparateInstrumentationKey()
        {
            var diagnosticsInstrumentationKey = Guid.NewGuid().ToString();
            this.nonThrottlingPortalSender.DiagnosticsInstrumentationKey = diagnosticsInstrumentationKey;

            var evt = EventWrittenEventArgsMock.GetInstance(
                level: EventLevel.Warning,
                message: "Error occurred at {0}, {1}",
                payload: new ReadOnlyCollection<object>(new List<object>() { "My function", "some failure" }));

            this.nonThrottlingPortalSender.Send(evt);

            Assert.AreEqual(1, this.sendItems.Count);
            var trace = this.sendItems[0] as TraceTelemetry;
            Assert.IsNotNull(trace);
            Assert.AreEqual(diagnosticsInstrumentationKey, trace.Context.InstrumentationKey);
            Assert.AreEqual("SDKTelemetry", trace.Context.Operation.SyntheticSource);
        }

        [TestMethod]
        public void TestSendingEmptyPayload()
        {
            var evt = EventWrittenEventArgsMock.GetInstance(
                level: EventLevel.Warning,
                message: "Something failed",
                payload: null);

            this.nonThrottlingPortalSender.Send(evt);

            Assert.AreEqual(1, this.sendItems.Count);
            var trace = this.sendItems[0] as TraceTelemetry;
            Assert.IsNotNull(trace);
            Assert.AreEqual(
                "AI (Internal): [] Something failed",
                trace.Message);
            Assert.AreEqual(0, trace.Properties.Count);
        }

        [TestMethod]
        public void SendNotFailIfChannelNotInitialized()
        {
            var configuration = new TelemetryConfiguration();
            var portalSenderWithDefaultCOnfiguration = new PortalDiagnosticsSender(
                configuration,
                this.dontThrottleManager);

            var evt = EventWrittenEventArgsMock.GetInstance(
                level: EventLevel.Warning,
                message: "Something failed",
                payload: null);

            portalSenderWithDefaultCOnfiguration.Send(evt);
        }
    }
}
