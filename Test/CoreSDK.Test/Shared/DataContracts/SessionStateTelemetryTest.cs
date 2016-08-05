namespace Microsoft.ApplicationInsights.DataContracts
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class SessionStateTelemetryTest
    {
#pragma warning disable 618
        [TestMethod]
        public void SessionStateTelemetryImplementsITelemetryContract()
        {
            var test = new ITelemetryTest<SessionStateTelemetry, AI.EventData>();
            test.Run();
        }

        [TestMethod]
        public void ConstructorInitializesStateWithSpecifiedValue()
        {
            var telemetry = new SessionStateTelemetry(SessionState.End);
            Assert.Equal(SessionState.End, telemetry.State);
        }

        [TestMethod]
        public void ParameterizedConstructorPerformsDefaultInitialization()
        {
            var telemetry = new SessionStateTelemetry(SessionState.Start);
            Assert.NotNull(telemetry.Context);
        }

        [TestMethod]
        public void SessionStateIsStartByDefault()
        {
            var telemetry = new SessionStateTelemetry();
            Assert.Equal(SessionState.Start, telemetry.State);
        }

        [TestMethod]
        public void SessionStateCanBeSetByUser()
        {
            var telemetry = new SessionStateTelemetry();
            telemetry.State = SessionState.End;
            Assert.Equal(SessionState.End, telemetry.State);
        }

        [TestMethod]
        public void SerializeWritesStateAsExpectedByEndpoint()
        {
            var telemetry = new SessionStateTelemetry { State = SessionState.End };
            AI.TelemetryItem<AI.EventData> envelope = TelemetryItemTestHelper.SerializeDeserializeTelemetryItem<SessionStateTelemetry, AI.EventData>(telemetry);
            Assert.Equal("End", envelope.data.baseData.properties["State"]);
            Assert.Equal(2, envelope.data.baseData.ver);
        }
#pragma warning restore 618
    }
}
