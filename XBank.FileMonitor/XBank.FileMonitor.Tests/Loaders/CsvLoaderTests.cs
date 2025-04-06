using System.IO;
using System.Text;
using System.Linq;
using XBank.FileMonitor.Loaders;
using Xunit;
using XBank.FileMonitor.Loaders.Loaders;

namespace XBank.FileMonitor.Tests.Loaders
{
    public class CsvLoaderTests
    {
        [Fact]
        public void Load_ValidCsv_ReturnsCorrectData()
        {
            var csvContent = @"2013-5-20,30.16,30.39,30.02,30.17,1478200
2013-5-21,30.18,30.50,30.10,30.40,1678900";

            var loader = new CsvLoader();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

            var result = loader.Load(stream).ToList();

            Assert.Equal(2, result.Count);

            Assert.Equal(30.16m, result[0].Open);
            Assert.Equal(30.50m, result[1].High);
        }

        [Fact]
        public void Load_InvalidLine_ThrowsFormatException()
        {
            var invalidCsv = @"2013-5-20,30.16,INVALID,30.02,30.17,1478200";

            var loader = new CsvLoader();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidCsv));

            Assert.Throws<FormatException>(() => loader.Load(stream).ToList());
        }
    }
}
