using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using XBank.FileMonitor.Core.Interfaces;
using XBank.FileMonitor.Core.Models;

namespace XBank.FileMonitor.Loaders.Loaders
{
    public class XmlLoader : ILoader
    {
        public string FileExtension => ".xml";

        public IEnumerable<TradeData> Load(Stream fileStream)
        {
            try
            {
                var result = new List<TradeData>();
                var doc = XDocument.Load(fileStream);
                int elementIndex = 0;

                foreach (var element in doc.Descendants("value"))
                {
                    elementIndex++;
                    try
                    {
                        var trade = new TradeData
                        {
                            Date = DateTime.Parse(element.Attribute("date")?.Value ?? ""),
                            Open = decimal.Parse(element.Attribute("open")?.Value ?? "", CultureInfo.InvariantCulture),
                            High = decimal.Parse(element.Attribute("high")?.Value ?? "", CultureInfo.InvariantCulture),
                            Low = decimal.Parse(element.Attribute("low")?.Value ?? "", CultureInfo.InvariantCulture),
                            Close = decimal.Parse(element.Attribute("close")?.Value ?? "", CultureInfo.InvariantCulture),
                            Volume = long.Parse(element.Attribute("volume")?.Value ?? "", CultureInfo.InvariantCulture)
                        };

                        result.Add(trade);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Error parsing XML element #{elementIndex}: {element}", ex);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to load or parse XML document.", ex);
            }
        }
    }
}
