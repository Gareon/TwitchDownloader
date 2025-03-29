using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitchDownloaderCLI.Models;
using TwitchDownloaderCore.Interfaces;

namespace TwitchDownloaderCLI.Tools
{
    internal class CliJsonTaskProgress : ITaskProgress
    {
        private static JsonSerializerOptions _jsonOptions = new() { WriteIndented = false, PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private string _status;
        private bool _statusIsTemplate;

        private int _lastPercent = -1;
        private TimeSpan _lastTime1 = new(-1);
        private TimeSpan _lastTime2 = new(-1);

        private readonly LogLevel _logLevel;
        private readonly TextWriter _console;

        public CliJsonTaskProgress(LogLevel logLevel, ProgressReportStream reportStream)
        {
            if ((logLevel & LogLevel.None) == 0)
            {
                _logLevel = logLevel;
            }
            
            _console = reportStream switch { ProgressReportStream.Error => Console.Error, _ => Console.Out };
        }

        public void SetStatus(string status)
        {
            if ((_logLevel & LogLevel.Status) == 0) return;

            lock (this)
            {
                _status = status;
                _statusIsTemplate = false;

                WriteJsonProgress(status, null, null, null);
            }
        }

        public void SetTemplateStatus(string status, int initialPercent)
        {
            if ((_logLevel & LogLevel.Status) == 0) return;

            lock (this)
            {
                _status = status;
                _statusIsTemplate = true;

                _lastPercent = -1; // Ensure that the progress report runs
                ReportProgress(initialPercent);
            }
        }

        public void SetTemplateStatus(string status, int initialPercent, TimeSpan initialTime1, TimeSpan initialTime2)
        {
            if ((_logLevel & LogLevel.Status) == 0) return;

            lock (this)
            {
                _status = status;
                _statusIsTemplate = true;

                _lastPercent = -1; // Ensure that the progress report runs
                ReportProgress(initialPercent, initialTime1, initialTime2);
            }
        }

        public void ReportProgress(int percent)
        {
            if ((_logLevel & LogLevel.Status) == 0) return;

            lock (this)
            {
                if (_lastPercent == percent
                    || !_statusIsTemplate)
                {
                    return;
                }

                WriteJsonProgress(_status, percent, null, null);
                _lastPercent = percent;
            }
        }

        public void ReportProgress(int percent, TimeSpan time1, TimeSpan time2)
        {
            if ((_logLevel & LogLevel.Status) == 0) return;

            lock (this)
            {
                if ((_lastPercent == percent && _lastTime1 == time1 && _lastTime2 == time2)
                    || !_statusIsTemplate)
                {
                    return;
                }

                WriteJsonProgress(_status, percent, time1, time2);
                
                _lastPercent = percent;
                _lastTime1 = time1;
                _lastTime2 = time2;
            }
        }

        private void WriteJsonProgress(string status, int? percent, TimeSpan? elapsed, TimeSpan? eta)
        {
            _console.WriteLine(JsonSerializer.Serialize(new { Type = "Progress", Status = status, Percent = percent, Elapsed = elapsed, Eta = eta }, _jsonOptions));
        }
        
        public void LogVerbose(string logMessage)
        {
            if ((_logLevel & LogLevel.Verbose) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Verbose, logMessage);
            }
        }

        public void LogVerbose(DefaultInterpolatedStringHandler logMessage)
        {
            if ((_logLevel & LogLevel.Verbose) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Verbose, logMessage.ToStringAndClear());
            }
        }

        public void LogInfo(string logMessage)
        {
            if ((_logLevel & LogLevel.Info) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Info, logMessage);
            }
        }

        public void LogInfo(DefaultInterpolatedStringHandler logMessage)
        {
            if ((_logLevel & LogLevel.Info) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Info, logMessage.ToStringAndClear());
            }
        }

        public void LogWarning(string logMessage)
        {
            if ((_logLevel & LogLevel.Warning) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Warning, logMessage);
            }
        }

        public void LogWarning(DefaultInterpolatedStringHandler logMessage)
        {
            if ((_logLevel & LogLevel.Warning) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Warning, logMessage.ToStringAndClear());
            }
        }

        public void LogError(string logMessage)
        {
            if ((_logLevel & LogLevel.Error) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Error, logMessage);
            }
        }

        public void LogError(DefaultInterpolatedStringHandler logMessage)
        {
            if ((_logLevel & LogLevel.Error) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Error, logMessage.ToStringAndClear());
            }
        }

        public void LogFfmpeg(string logMessage)
        {
            if ((_logLevel & LogLevel.Ffmpeg) == 0) return;

            lock (this)
            {
                WriteLogMessage(LogLevel.Ffmpeg, logMessage);
            }
        }

        private void WriteLogMessage(LogLevel logLevel, string message)
        {
            _console.WriteLine(JsonSerializer.Serialize(new { Type = "Log", Level = logLevel, Message = message }, _jsonOptions));
        }
    }
}