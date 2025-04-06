using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XBank.FileMonitor.Core.Interfaces;
using XBank.FileMonitor.Core.Models;

namespace XBank.FileMonitor.App.Services
{
    public class FileMonitorService
    {
        private readonly string _directoryPath;
        private readonly int _pollingIntervalSeconds;
        private readonly List<ILoader> _loaders;
        private readonly Action<TradeData, string> _onDataLoaded;
        private readonly Action<string> _onFileStart;
        private readonly Action _onFileEnd;
        private readonly Action<string, string> _onFileError;


        private readonly HashSet<string> _processedFiles = new();
        private CancellationTokenSource _cts;

        public FileMonitorService(
    string directoryPath,
    int pollingIntervalSeconds,
    List<ILoader> loaders,
    Action<TradeData, string> onDataLoaded,
    Action<string> onFileStart,
    Action onFileEnd,
    Action<string, string> onFileError)
        {
            _directoryPath = directoryPath;
            _pollingIntervalSeconds = pollingIntervalSeconds;
            _loaders = loaders;
            _onDataLoaded = onDataLoaded;
            _onFileStart = onFileStart;
            _onFileEnd = onFileEnd;
            _onFileError = onFileError;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => MonitorLoop(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private async Task MonitorLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var files = Directory.GetFiles(_directoryPath);

                    var newFiles = files.Where(f => !_processedFiles.Contains(f)).ToList();

                    var tasks = newFiles.Select(file => Task.Run(() => ProcessFile(file), token)).ToArray();
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "MonitorLoop error");
                }


                await Task.Delay(TimeSpan.FromSeconds(_pollingIntervalSeconds), token);
            }
        }

        private void ProcessFile(string filePath)
        {
            try
            {
                _onFileStart?.Invoke(Path.GetFileName(filePath));

                var extension = Path.GetExtension(filePath);
                var loader = _loaders.FirstOrDefault(l => l.FileExtension.Equals(extension, StringComparison.OrdinalIgnoreCase));
                if (loader == null)
                    return;

                using var stream = File.OpenRead(filePath);
                var dataList = loader.Load(stream);

                foreach (var data in dataList)
                {
                    _onDataLoaded?.Invoke(data, Path.GetFileName(filePath));
                }

                _processedFiles.Add(filePath);
            }
            catch (Exception ex)
            {
                _onFileError?.Invoke(Path.GetFileName(filePath), ex.Message);
            }
            finally
            {
                _onFileEnd?.Invoke();
            }
        }

    }
}
