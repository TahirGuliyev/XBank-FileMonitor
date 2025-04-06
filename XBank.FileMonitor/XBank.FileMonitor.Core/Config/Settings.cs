using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBank.FileMonitor.Core.Config
{
    public class Settings
    {
        public string PluginsDirectory { get; set; }
        public int PollingIntervalSeconds { get; set; }
        public string WatchDirectory { get; set; }
        public List<string> ActiveLoaders { get; set; }
    }
}
