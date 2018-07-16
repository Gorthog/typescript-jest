namespace BytesUtilities
{
    public class HumanReadableBytesSize
    {
        public string BytesToString(long bytesCount)
        {
            var readableSizeSuffix = new string[] { "B", "KB", "MB", "GB", "TB", "ZB", "EB" };
            var readableSizeSuffixIndex = 0;
            decimal readableSizeNumber = bytesCount;
            while (readableSizeNumber >= 1024)
            {
                readableSizeNumber /= 1024;
                readableSizeSuffixIndex++;
            }

            return $"{readableSizeNumber:0.##}{readableSizeSuffix[readableSizeSuffixIndex]}";
        }
    }
}
