namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Diagnostics.Tracing;

    internal static class EventSources
    {
        /// <summary>
        /// This method checks if the given EventSource Name matches known EventSources that we want to subscribe to.
        /// </summary>
        public static bool ShouldSubscribe(EventSource eventSource)
        {
            if (eventSource.Name.StartsWith("Microsoft-A", StringComparison.Ordinal))
            {
                switch (eventSource.Name)
                {
                    case "Microsoft-ApplicationInsights-Core": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/BASE/src/Microsoft.ApplicationInsights/Extensibility/Implementation/Tracing/CoreEventSource.cs
                    case "Microsoft-ApplicationInsights-WindowsServer-TelemetryChannel": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/BASE/src/ServerTelemetryChannel/Implementation/TelemetryChannelEventSource.cs

                    // AppMapCorrelation has a shared partial class: https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/Common/AppMapCorrelationEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation-Dependency": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/DependencyCollector/DependencyCollector/Implementation/AppMapCorrelationEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-AppMapCorrelation-Web": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/Web/Web/Implementation/AppMapCorrelationEventSource.cs

                    case "Microsoft-ApplicationInsights-Extensibility-DependencyCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/DependencyCollector/DependencyCollector/Implementation/DependencyCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-EventCounterCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/EventCounterCollector/EventCounterCollector/EventCounterCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/PerformanceCollector/PerformanceCollector/Implementation/PerformanceCollectorEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-PerformanceCollector-QuickPulse": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/PerformanceCollector/PerformanceCollector/Implementation/QuickPulse/QuickPulseEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-Web": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/Web/Web/Implementation/WebEventSource.cs
                    case "Microsoft-ApplicationInsights-Extensibility-WindowsServer": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/WindowsServer/WindowsServer/Implementation/WindowsServerEventSource.cs
                    case "Microsoft-ApplicationInsights-WindowsServer-Core": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/WEB/Src/WindowsServer/WindowsServer/Implementation/MetricManager.cs
                    case "Microsoft-ApplicationInsights-Extensibility-EventSourceListener": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/LOGGING/src/EventSource.Shared/EventSource.Shared/Implementation/EventSourceListenerEventSource.cs
                    case "Microsoft-ApplicationInsights-AspNetCore": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/master/NETCORE/src/Microsoft.ApplicationInsights.AspNetCore/Extensibility/Implementation/Tracing/AspNetCoreEventSource.cs
                    case "Microsoft-ApplicationInsights-LoggerProvider": // https://github.com/microsoft/ApplicationInsights-dotnet/blob/develop/LOGGING/src/ILogger/ApplicationInsightsLoggerEventSource.cs
                    case "Microsoft-AspNet-Telemetry-Correlation": // https://github.com/aspnet/Microsoft.AspNet.TelemetryCorrelation/blob/master/src/Microsoft.AspNet.TelemetryCorrelation/AspNetTelemetryCorrelationEventSource.cs
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
    }
}
