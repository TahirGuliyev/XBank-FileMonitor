using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using XBank.FileMonitor.App.Services;
using XBank.FileMonitor.App.ViewModels;
using XBank.FileMonitor.Core.Models;

namespace XBank.FileMonitor.App
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TradeDataViewModel> TradeDataList { get; set; } = new();

        private FileMonitorService _monitorService;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var settings = ConfigurationService.LoadSettings();

            var loaders = PluginLoader.LoadPlugins(settings.PluginsDirectory, settings.ActiveLoaders).ToList();

            _monitorService = new FileMonitorService(
                settings.WatchDirectory,
                settings.PollingIntervalSeconds,
                loaders,
                OnDataLoaded,
                ShowLoading,
                HideLoading,
                ShowError);

            _monitorService.Start();
        }

        private void OnDataLoaded(TradeData data, string sourceFile)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TradeDataList.Add(new TradeDataViewModel
                {
                    Date = data.Date,
                    Open = data.Open,
                    High = data.High,
                    Low = data.Low,
                    Close = data.Close,
                    Volume = data.Volume,
                    SourceFile = sourceFile
                });
            });
        }

        private void ShowLoading(string fileName)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = $"Processing file: {fileName}";
                LoadingProgressBar.Visibility = Visibility.Visible;
            });
        }

        private void HideLoading()
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBlock.Text = "Ready";
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            });
        }

        private void ShowError(string fileName, string errorMessage)
        {
            Dispatcher.Invoke(() =>
            {
                string fullMessage = $"[{DateTime.Now:HH:mm:ss}] Failed to process '{fileName}': {errorMessage}";
                ErrorListBox.Items.Add(fullMessage);
                ErrorListBox.ScrollIntoView(ErrorListBox.Items[ErrorListBox.Items.Count - 1]);
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            _monitorService.Stop();
            base.OnClosed(e);
        }
    }
}
