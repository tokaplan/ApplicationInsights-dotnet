namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Tracing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;

    using Moq;

    [TestClass]
    public class F5DiagnosticsSenderTest
    {
        [TestMethod]
        public void TestLogMessage()
        {
            var senderMock = new DiagnosticsSenderMock();

            var evt = EventWrittenEventArgsMock.GetInstance(
                level: EventLevel.Warning,
                message: "Error occurred at {0}, {1}",
                payload: new ReadOnlyCollection<object>(new List<object>() { "My function", "some failure" }));

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("Error occurred at My function, some failure", senderMock.Messages[0]);
        }

        [TestMethod]
        public void TestLogMessageWithEmptyPayload()
        {
            var senderMock = new DiagnosticsSenderMock();

            var evt = EventWrittenEventArgsMock.GetInstance(
                level: EventLevel.Warning,
                message: "Error occurred",
                payload: null);

            senderMock.Send(evt);
            Assert.AreEqual(1, senderMock.Messages.Count);
            Assert.AreEqual("Error occurred", senderMock.Messages[0]);
        }
    }
}
