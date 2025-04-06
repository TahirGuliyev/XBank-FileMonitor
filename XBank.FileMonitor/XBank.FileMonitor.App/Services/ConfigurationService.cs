using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using XBank.FileMonitor.Core.Config;

namespace XBank.FileMonitor.App.Services
{
    public static class ConfigurationService
    {
        public static Settings LoadSettings(string path = "appsettings.json")
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Settings>(json);
        }
    }
}
