namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.IO;

    internal class SelfDiagnosticsFileWriter : ISelfDiagnosticsFileWriter, IDisposable
    {
        private SDEventListener eventListener;
        private bool disposedValue;

        public void Initialize(string level, string fileDirectory, long maxFileSizeBytes)
        {
            if (string.IsNullOrWhiteSpace(fileDirectory))
            {
                throw new ArgumentNullException(nameof(fileDirectory));
            }


            if (!Enum.TryParse(level, out EventLevel eventLevel))
            {
                eventLevel = SelfDiagnosticsProvider.DefaultLevel;
            }

            // TODO: THIS CAN BE LARGELY REMOVED BY MOVING VALIDATION INTO THE PROVIDER.
            string expandedDirectory = Environment.ExpandEnvironmentVariables(fileDirectory);
            if (IsValidLogsDirectory(expandedDirectory))
            {
                this.eventListener = new SDEventListener(eventLevel, new FileWriter(expandedDirectory, maxFileSizeBytes));
            }
            else if(IsValidLogsDirectory(SelfDiagnosticsProvider.DefaultDirectory))
            {
                this.eventListener = new SDEventListener(eventLevel, new FileWriter(SelfDiagnosticsProvider.DefaultDirectory, maxFileSizeBytes));
            }
            else
            {
                // ERROR STATE
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.eventListener.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private static bool IsValidLogsDirectory(string logDirectory)
        {
            bool result = false;
            try
            {
                FileHelper.TestDirectoryPermissions(new DirectoryInfo(logDirectory));
                result = true;
            }
            catch (Exception)
            {
                // no op
            }

            return result;
        }
    }
}
