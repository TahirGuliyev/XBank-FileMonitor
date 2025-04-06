using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using XBank.FileMonitor.Core.Interfaces;
using XBank.FileMonitor.Core.Models;

namespace XBank.FileMonitor.Loaders.Loaders
{
    public class CsvLoader : ILoader
    {
        public string FileExtension => ".csv";

        public IEnumerable<TradeData> Load(Stream fileStream)
        {
            var result = new List<TradeData>();
            using var reader = new StreamReader(fileStream);
            int lineNumber = 0;

            while (!reader.EndOfStream)
            {
                lineNumber++;
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');
                if (parts.Length != 6)
                    throw new FormatException($"Line {lineNumber} has incorrect number of fields: {line}");

                try
                {
                    var trade = new TradeData
                    {
                        Date = DateTime.Parse(parts[0]),
                        Open = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
                        High = decimal.Parse(parts[2], CultureInfo.InvariantCulture),
                        Low = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
                        Close = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                        Volume = long.Parse(parts[5], CultureInfo.InvariantCulture)
                    };

                    result.Add(trade);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Error parsing CSV line {lineNumber}: {line}", ex);
                }
            }

            return result;
        }
    }
}
