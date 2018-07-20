using System;

namespace SpanExtensions
{
    public static class SpanStringExtensions
    {
        static string[] SingleSeparatorBuffer = new string[1];
        public static SpanSplitEnumerable Split(this in ReadOnlySpan<char> span, string separator)
        {
            SingleSeparatorBuffer[0] = separator;
            return new SpanSplitEnumerable(span, SingleSeparatorBuffer);
        }

        public static SpanSplitEnumerable Split(this in ReadOnlySpan<char> span, string[] separators)
        {
            return new SpanSplitEnumerable(span, separators);
        }
    }
}
