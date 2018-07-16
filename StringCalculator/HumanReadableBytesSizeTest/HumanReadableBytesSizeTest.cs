using BytesUtilities;
using FluentAssertions;
using Xunit;

namespace HumanReadableBytesSizeTest
{
    public class HumanReadableBytesSizeTest
    {
        HumanReadableBytesSize humanReadableBytes;

        public HumanReadableBytesSizeTest()
        {
            humanReadableBytes = new HumanReadableBytesSize();
        }

        [Theory]
        [InlineData(0, "0B")]
        [InlineData(1024, "1KB")]
        [InlineData(1024 * 1024, "1MB")]
        [InlineData(2000000, "1.91MB")]
        void ItReturnsCorrectMessage(long bytesCount, string expectedMessage)
        {
            string readableSize = humanReadableBytes.BytesToString(bytesCount);

            readableSize.Should().Be(expectedMessage);
        }
    }
}
