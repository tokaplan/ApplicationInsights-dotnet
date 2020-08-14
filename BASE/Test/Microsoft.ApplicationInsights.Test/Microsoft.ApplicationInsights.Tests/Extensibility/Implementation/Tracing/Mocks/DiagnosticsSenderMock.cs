namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;
    using TestFramework;

    internal class DiagnosticsSenderMock : IDiagnosticsSender
    {
        public IList<string> Messages = new List<string>();

        public void Send(EventWrittenEventArgs eventData)
        {
            var message = eventData.Payload != null && eventData.Payload.Count > 0 ?
                string.Format(CultureInfo.InvariantCulture, eventData.Message, eventData.Payload.ToArray()) :
                eventData.Message;

            this.Messages.Add(message);
        }
    }
}
