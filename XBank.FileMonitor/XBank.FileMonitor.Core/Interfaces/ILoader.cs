using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XBank.FileMonitor.Core.Interfaces
{
    public interface ILoader
    {
        string FileExtension { get; }
        IEnumerable<Models.TradeData> Load(Stream fileStream);
    }
}
