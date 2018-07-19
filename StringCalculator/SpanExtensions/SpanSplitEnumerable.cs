
// https://gist.github.com/LordJZ/e0b5245d69497f2a43a5f09c1d26e34c
using System;

namespace SpanExtensions
{
    public ref struct SpanSplitEnumerable
    {
        ReadOnlySpan<char> Span { get; }
        ReadOnlySpan<char> Separator { get; }
        public SpanSplitEnumerable(in ReadOnlySpan<char> span, in ReadOnlySpan<char> separator)
        {
            Span = span;
            Separator = separator;
        }

        public SpanSplitEnumerator GetEnumerator()
            => new SpanSplitEnumerator(Span, Separator);
    }
}