using CommandLine;
using TwitchDownloaderCLI.Models;

namespace TwitchDownloaderCLI.Modes.Arguments
{
    internal interface ITwitchDownloaderArgs
    {
        [Option("banner", Default = true, HelpText = "Displays a banner containing version and copyright information.")]
        public bool? ShowBanner { get; set; }

        [Option("log-level", Default = LogLevel.Status | LogLevel.Info | LogLevel.Warning | LogLevel.Error, HelpText = "Sets the log level flags. Applicable values are: None, Status, Verbose, Info, Warning, Error, Ffmpeg.")]
        public LogLevel LogLevel { get; set; }
        
        [Option("progress-report-format", Default = ProgressReportFormat.Default, HelpText = "Sets the format of the progress report. Applicable values are: Default, Json.")]
        public ProgressReportFormat ProgressReportFormat { get; set; }
        
        [Option("progress-report-stream", Default = ProgressReportStream.Out, HelpText = "Sets the console stream to write the progress report to. Applicable values are: Out, Error.")]
        public ProgressReportStream ProgressReportStream { get; set; }
    }
}