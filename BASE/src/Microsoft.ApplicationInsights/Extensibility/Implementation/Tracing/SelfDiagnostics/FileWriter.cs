using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    internal class FileWriter
    {
        private readonly object lockObj = new object();
        private readonly long maxFileSizeBytes;
        private readonly string logDirectory;
        private string logFilePath;

        public FileWriter(string logDirectory, long maxFileSizeBytes)
        {
            this.logDirectory = logDirectory;
            this.maxFileSizeBytes = maxFileSizeBytes;

            lock(this.lockObj)
            {
                this.StartNewFile();
            }
        }

        public void WriteLine(string line)
        {
            try
            {
                lock (this.lockObj)
                {
                    // Write
                    FileInfo file = new FileInfo(this.logFilePath);
                    using (Stream stream = file.Open(FileMode.OpenOrCreate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            stream.Position = stream.Length;
                            writer.WriteLine(line);
                        }
                    }

                    // Check File Size and start new if necessary.
                    if (file.Length > maxFileSizeBytes)
                    {
                        this.StartNewFile();
                    }
                }
            }
            catch (Exception)
            {
                // no op
            }
        }

        private void StartNewFile()
        {
            this.logFilePath = Path.Combine(this.logDirectory, FileHelper.GenerateFileName());

            string[] lines =
            {
                ".NET SDK version: " + SdkVersionUtils.GetSdkVersion(string.Empty),
                string.Empty,
            };

            File.WriteAllLines(logFilePath, lines);
        }
    }
}
