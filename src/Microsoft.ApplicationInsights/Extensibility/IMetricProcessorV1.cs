namespace Microsoft.ApplicationInsights.Extensibility
{
    /// <summary>
    /// Provides functionality to process metric values prior to aggregation.
    /// </summary>
    internal interface IMetricProcessorV1
    {
        /// <summary>
        /// Process metric value.
        /// </summary>
        /// <param name="metric">MetricV1 definition.</param>
        /// <param name="value">MetricV1 value.</param>
        void Track(MetricV1 metric, double value);
    }
}
