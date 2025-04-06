using System.IO;
using System.Text;
using System.Linq;
using XBank.FileMonitor.Loaders;
using Xunit;
using XBank.FileMonitor.Loaders.Loaders;

namespace XBank.FileMonitor.Tests.Loaders
{
    public class XmlLoaderTests
    {
        [Fact]
        public void Load_ValidXml_ReturnsCorrectData()
        {
            var xmlContent = @"<root>
<value date='2013-5-20' open='30.16' high='30.39' low='30.02' close='30.17' volume='1478200' />
<value date='2013-5-21' open='30.18' high='30.50' low='30.10' close='30.40' volume='1678900' />
</root>";

            var loader = new XmlLoader();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));

            var result = loader.Load(stream).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(30.16m, result[0].Open);
            Assert.Equal(30.50m, result[1].High);
        }

        [Fact]
        public void Load_InvalidXml_ThrowsFormatException()
        {
            var xmlContent = @"<root>
<value date='2013-5-20' open='INVALID' high='30.39' low='30.02' close='30.17' volume='1478200' />
</root>";

            var loader = new XmlLoader();
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent));

            Assert.Throws<FormatException>(() => loader.Load(stream).ToList());
        }
    }
}
