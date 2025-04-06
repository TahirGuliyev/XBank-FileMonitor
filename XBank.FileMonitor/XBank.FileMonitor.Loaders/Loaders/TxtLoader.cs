using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XBank.FileMonitor.Core.Interfaces;
using XBank.FileMonitor.Core.Models;

namespace XBank.FileMonitor.Loaders.Loaders
{
    public class TxtLoader : ILoader
    {
        public string FileExtension => ".txt";

        public IEnumerable<TradeData> Load(Stream fileStream)
        {
            var result = new List<TradeData>();

            using var reader = new StreamReader(fileStream);

            bool isFirstLine = true;
            int lineNumber = 0;

            while (!reader.EndOfStream)
            {
                lineNumber++;

                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length != 6)
                {
                    throw new FormatException($"Line {lineNumber} has incorrect number of fields: {line}");
                }

                try
                {
                    var trade = new TradeData
                    {
                        Date = DateTime.Parse(parts[0]),
                        Open = decimal.Parse(parts[1]),
                        High = decimal.Parse(parts[2]),
                        Low = decimal.Parse(parts[3]),
                        Close = decimal.Parse(parts[4]),
                        Volume = long.Parse(parts[5])
                    };
                    result.Add(trade);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Error parsing line {lineNumber}: {line}", ex);
                }
            }

            return result;
        }
    }
}
