namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track user sessions.
    /// </summary>
    [Obsolete("Session state telemetry is no longer being used by Application Insights.")]
    public sealed class SessionStateTelemetry : ITelemetry
    {
        internal const string TelemetryName = "SessionState";

        internal EventTelemetry EventTelemetry;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateTelemetry"/> class.
        /// </summary>
        public SessionStateTelemetry()
        {
            this.EventTelemetry = new EventTelemetry("Session state");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateTelemetry"/> class with the specified <paramref name="state"/>.
        /// </summary>
        /// <param name="state">
        /// A <see cref="SessionState"/> value indicating state of the user session.
        /// </param>
        public SessionStateTelemetry(SessionState state) : this()
        {
            this.State = state;
        }

        /// <summary>
        /// Gets or sets the date and time the session state was recorded.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
                return this.EventTelemetry.Timestamp;
            }

            set
            {
                this.EventTelemetry.Timestamp = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="TelemetryContext"/> of the application when the session state was recorded.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.EventTelemetry.Context; }
        }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence
        {
            get
            {
                return this.EventTelemetry.Sequence;
            }

            set
            {
                this.EventTelemetry.Sequence = value;
            }
        }

        /// <summary>
        /// Gets or sets the value describing state of the user session.
        /// </summary>
        public SessionState State
        {
            get
            {
                SessionState result = SessionState.Start;
                if (this.EventTelemetry.Properties.ContainsKey("state"))
                {
                    var state = this.EventTelemetry.Properties["state"];
                    Enum.TryParse<SessionState>(state, out result);
                }

                return result;
            }

            set
            {
                this.EventTelemetry.Properties["state"] = value.ToString();
            }
        }

        /// <summary>
        /// Sanitizes this telemetry instance to ensure it can be accepted by the Application Insights.
        /// </summary>
        void ITelemetry.Sanitize()
        {
        }
    }
}
