// -----------------------------------------------------------------------
// <copyright file="DiagnosticsEventCollectingMock.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>
// <author>Sergei Nikitin: sergeyni@microsoft.com</author>
// <summary></summary>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnosticsModule;

    internal class DiagnosticsEventCollectingMock : IDiagnosticsSender
    {
        private readonly IList<EventWrittenEventArgs> eventList = new List<EventWrittenEventArgs>();

        public IList<EventWrittenEventArgs> EventList
        {
            get { return this.eventList; }
        }

        public void Send(EventWrittenEventArgs eventData)
        {
            lock (this.eventList)
            {
                this.EventList.Add(eventData);
            }
        }
    }
}
