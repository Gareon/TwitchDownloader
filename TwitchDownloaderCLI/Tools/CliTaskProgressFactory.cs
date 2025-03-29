using TwitchDownloaderCLI.Models;
using TwitchDownloaderCore.Interfaces;

namespace TwitchDownloaderCLI.Tools
{
    internal static class CliTaskProgressFactory
    {
        public static ITaskProgress Create(LogLevel logLevel, ProgressReportStream reportStream, ProgressReportFormat reportFormat)
        {
            return reportFormat switch
            {
                ProgressReportFormat.Json => new CliJsonTaskProgress(logLevel, reportStream),
                _ => new CliTaskProgress(logLevel, reportStream)
            };
        }
    }
}