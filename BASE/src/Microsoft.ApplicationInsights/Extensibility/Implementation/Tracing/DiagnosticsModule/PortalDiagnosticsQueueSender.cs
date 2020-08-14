namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    /// <summary>
    /// A dummy queue sender to keep the data to be sent to the portal before the initialize method is called.
    /// This is due to the fact that initialize method cannot be called without the configuration and 
    /// the event listener write event is triggered before the diagnosticTelemetryModule initialize method is triggered.
    /// </summary>
    internal class PortalDiagnosticsQueueSender : IDiagnosticsSender
    {
        public PortalDiagnosticsQueueSender()
        {
            this.EventData = new List<EventWrittenEventArgs>();
            this.IsDisabled = false;
        }

        public IList<EventWrittenEventArgs> EventData { get; }

        public bool IsDisabled { get; set; }

        public void Send(EventWrittenEventArgs eventWrittenEventArgs)
        {
            if (!this.IsDisabled)
            {
                this.EventData.Add(eventWrittenEventArgs);
            }
        }

        public void FlushQueue(IDiagnosticsSender sender)
        {
            foreach (var traceEvent in this.EventData)
            {
                sender.Send(traceEvent);
            }
        }
    }
}
