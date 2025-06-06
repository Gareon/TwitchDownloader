﻿using System.Threading;
using TwitchDownloaderCLI.Modes.Arguments;
using TwitchDownloaderCLI.Tools;
using TwitchDownloaderCore;
using TwitchDownloaderCore.Options;

namespace TwitchDownloaderCLI.Modes
{
    internal static class MergeTs
    {
        internal static void Merge(TsMergeArgs inputOptions)
        {
            var progress = CliTaskProgressFactory.Create(inputOptions.LogLevel, inputOptions.ProgressReportStream, inputOptions.ProgressReportFormat);

            progress.LogInfo("The TS merger is experimental and is subject to change without notice in future releases.");

            var collisionHandler = new FileCollisionHandler(inputOptions, progress);
            var mergeOptions = GetMergeOptions(inputOptions, collisionHandler);

            var tsMerger = new TsMerger(mergeOptions, progress);
            tsMerger.MergeAsync(new CancellationToken()).Wait();
        }

        private static TsMergeOptions GetMergeOptions(TsMergeArgs inputOptions, FileCollisionHandler collisionHandler)
        {
            TsMergeOptions mergeOptions = new()
            {
                OutputFile = inputOptions.OutputFile,
                InputFile = inputOptions.InputList,
                FileCollisionCallback = collisionHandler.HandleCollisionCallback,
            };

            return mergeOptions;
        }
    }
}
