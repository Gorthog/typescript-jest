using System;

namespace SpanExtensions
{
    public static class SpanStringExtensions
    {
        public static SpanSplitEnumerable Split(this in ReadOnlySpan<char> span, in ReadOnlySpan<char> separator)
        {
            return new SpanSplitEnumerable(span, separator);
        }
    }
}
