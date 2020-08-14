namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.DataContracts;

    using static System.FormattableString;

    internal static class EventWrittenEventArgsExtensions
    {
        private const string SdkTelemetrySyntheticSourceName = "SDKTelemetry";

        /// <summary>Prefix for user-actionable traces.</summary>
        private const string AiPrefix = "AI:";

        /// <summary>Prefix for non user-actionable traces. "AI Internal".</summary>
        private const string AiNonUserActionable = "AI (Internal):";

        public static TraceTelemetry ToTraceTelemetry(this EventWrittenEventArgs eventWrittenEventArgs, string diagnosticsInstrumentationKey)
        {
            var traceTelemetry = new TraceTelemetry(eventWrittenEventArgs.ToMessage());

            if (!string.IsNullOrEmpty(diagnosticsInstrumentationKey))
            {
                traceTelemetry.Context.InstrumentationKey = diagnosticsInstrumentationKey;
            }

            traceTelemetry.Context.Operation.SyntheticSource = SdkTelemetrySyntheticSourceName;

            return traceTelemetry;
        }

        private static string ToMessage(this EventWrittenEventArgs eventWrittenEventArgs)
        {
            // Add "AI: " prefix (if keyword does not contain UserActionable = (EventKeywords)0x1, than prefix should be "AI (Internal):" )
            var keywords = (long)eventWrittenEventArgs.Keywords;
            bool IsUserActionable = (keywords & EventSourceKeywords.UserActionable) == EventSourceKeywords.UserActionable;
            string messagePrefix = IsUserActionable ? AiPrefix : AiNonUserActionable;

            var EventSourceName = eventWrittenEventArgs.EventSource?.Name;
            var MessageFormat = eventWrittenEventArgs.Message;
            var Payload = eventWrittenEventArgs.Payload?.ToArray();
            string messageBody = Payload != null ?
                    string.Format(CultureInfo.CurrentCulture, MessageFormat, Payload.ToArray()) :
                    MessageFormat;

            string message = Invariant($"{messagePrefix} [{EventSourceName}] {messageBody}");

            return message;
        }
    }
}
