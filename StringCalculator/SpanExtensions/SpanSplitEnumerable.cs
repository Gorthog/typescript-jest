
// https://gist.github.com/LordJZ/e0b5245d69497f2a43a5f09c1d26e34c
using System;

namespace SpanExtensions
{
    public ref struct SpanSplitEnumerable
    {
        ReadOnlySpan<char> Span { get; }
        ReadOnlySpan<char> CombinedSeparator { get; }

        ReadOnlySpan<(int,int)> SeparatorsIndicesAndLengths { get; }
        public SpanSplitEnumerable(in ReadOnlySpan<char> span, in ReadOnlySpan<char> combinedSeparator, in ReadOnlySpan<(int,int)> separatorsIndices)
        {
            Span = span;
            CombinedSeparator = combinedSeparator;
            SeparatorsIndicesAndLengths = separatorsIndices;
        }

        public SpanSplitEnumerator GetEnumerator()
            => new SpanSplitEnumerator(Span, CombinedSeparator, SeparatorsIndicesAndLengths);
    }
}