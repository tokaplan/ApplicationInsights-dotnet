namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Microsoft.ApplicationInsights.Common.Extensions;

    using static System.FormattableString;

    internal class SDEventListener : EventListener
    {
        private readonly EventLevel level;
        private readonly FileWriter fileWriter;

        public SDEventListener(EventLevel level, FileWriter fileWriter)//string logFilePath)
        {
            this.level = level;
            //this.logFilePath = logFilePath;
            this.fileWriter = fileWriter;
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var messageBody = eventData.Payload != null ?
                string.Format(CultureInfo.CurrentCulture, eventData.Message, eventData.Payload.ToArray()) :
                eventData.Message;

            var message = Invariant($"{DateTime.UtcNow.ToInvariantString("o")}: {eventData.Level}: {messageBody}");

            fileWriter.WriteLine(message);
            //try
            //{
            //    lock (this.lockObj)
            //    {
            //        FileInfo file = new FileInfo(this.logFilePath);
            //        using (Stream stream = file.Open(FileMode.OpenOrCreate))
            //        {
            //            using (StreamWriter writer = new StreamWriter(stream))
            //            {
            //                stream.Position = stream.Length;
            //                writer.WriteLine(message);
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    // no op
            //}
        }

        /// <summary>
        /// This method subscribes on Application Insights EventSource.
        /// </summary>
        /// <param name="eventSource">EventSource to subscribe to.</param>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (EventSources.ShouldSubscribe(eventSource))
            {
#if NET452
                this.EnableEvents(eventSource, this.level, (EventKeywords)(-1));
#else
                this.EnableEvents(eventSource, this.level, EventKeywords.All);
#endif
            }

            base.OnEventSourceCreated(eventSource);
        }
    }
}
