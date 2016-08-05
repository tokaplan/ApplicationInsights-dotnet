namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;

    /// <summary>
    /// Contains values that identify state of a user session.
    /// </summary>
    [Obsolete("Session state events are no longer supported in Application Insights.")]
    public enum SessionState
    {
        /// <summary>
        /// Indicates that a user session started.
        /// </summary>
        Start,

        /// <summary>
        /// Indicates that a user session ended.
        /// </summary>
        End
    }
}
