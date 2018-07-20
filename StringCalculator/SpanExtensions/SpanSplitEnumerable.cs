
// https://gist.github.com/LordJZ/e0b5245d69497f2a43a5f09c1d26e34c
using System;

namespace SpanExtensions
{
    public ref struct SpanSplitEnumerable
    {
        ReadOnlySpan<char> Span { get; }
        ReadOnlySpan<string> Separators { get; }

        public SpanSplitEnumerable(in ReadOnlySpan<char> span, in ReadOnlySpan<string> separators)
        {
            Span = span;
            Separators = separators;
        }

        public SpanSplitEnumerator GetEnumerator()
            => new SpanSplitEnumerator(Span, Separators);
    }
}