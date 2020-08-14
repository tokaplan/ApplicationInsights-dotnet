namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System.Diagnostics.Tracing;

    internal interface IDiagnosticsSender
    {
        /// <summary>
        /// Sends diagnostics data to the appropriate output.
        /// </summary>
        /// <param name="eventWrittenEventArgs">Information about trace event.</param>
        void Send(EventWrittenEventArgs eventWrittenEventArgs);
    }
}