namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics.Tracing;
    using System.IO;

    internal class SelfDiagnosticsFileWriter : ISelfDiagnosticsFileWriter, IDisposable
    {
        private SDEventListener eventListener;
        private bool disposedValue;

        public void Initialize(string level, string fileDirectory)
        {
            string fileName = FileHelper.GenerateFileName();

            if (!Enum.TryParse(level, out EventLevel eventLevel))
            {
                eventLevel = SelfDiagnosticsProvider.DefaultLevel;
            }

            if (IsValidLogsFolder(fileDirectory, fileName, out string fullLogPath)
                || IsValidLogsFolder(SelfDiagnosticsProvider.DefaultDirectory, fileName, out fullLogPath))
            {
                WriteFileHeader(fullLogPath);
                this.eventListener = new SDEventListener(eventLevel, fullLogPath);
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

        private static void WriteFileHeader(string logFilePath)
        {
            string[] lines =
            {
                ".NET SDK version: " + SdkVersionUtils.GetSdkVersion(string.Empty),
                string.Empty,
            };

            File.WriteAllLines(logFilePath, lines);
        }

        private static bool IsValidLogsFolder(string fileDirectory, string fileName, out string fullLogPath)
        {
            bool result = false;
            fullLogPath = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(fileDirectory) && !string.IsNullOrWhiteSpace(fileName))
                {
                    string expandedDirectory = Environment.ExpandEnvironmentVariables(fileDirectory);
                    var logsDirectory = new DirectoryInfo(expandedDirectory);
                    FileHelper.TestDirectoryPermissions(logsDirectory);

                    fullLogPath = Path.Combine(expandedDirectory, fileName);
                    result = true;
                }
            }
            catch (Exception)
            {
                // no op
            }

            return result;
        }
    }
}
