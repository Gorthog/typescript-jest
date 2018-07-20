using System;

namespace SpanExtensions
{
    public static class SpanStringExtensions
    {
        public static SpanSplitEnumerable Split(this in ReadOnlySpan<char> span, in ReadOnlySpan<char> separator)
        {
            Span<(int, int)> separatorsIndicesAndLengths = new ValueTuple<int, int>[1]
                {
                    (0, separator.Length)
                };

            return new SpanSplitEnumerable(span, separator, separatorsIndicesAndLengths);
        }

        public static SpanSplitEnumerable Split(this in ReadOnlySpan<char> span, string[] separators)
        {
            Span<(int, int)> separatorsIndicesAndLengths = new ValueTuple<int, int>[separators.Length];
            int index = 0;
            for (int i = 0; i < separators.Length; i++)
            {
                separatorsIndicesAndLengths[i] = (index, separators[i].Length);
                index += separators[i].Length;
            }

            ReadOnlySpan<char> combinedSeparators = string.Join(string.Empty, separators);

            return new SpanSplitEnumerable(span, combinedSeparators, separatorsIndicesAndLengths);
        }
    }
}
