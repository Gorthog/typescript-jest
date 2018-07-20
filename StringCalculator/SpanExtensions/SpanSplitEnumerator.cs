using System;

namespace SpanExtensions
{
    public ref struct SpanSplitEnumerator
    {
        ReadOnlySpan<char> Span { get; set; }
        ReadOnlySpan<string> Separators { get; }
        public ReadOnlySpan<char> Current { get; private set; }


        public SpanSplitEnumerator(in ReadOnlySpan<char> span, in ReadOnlySpan<string> separators)
        {
            Span = span;
            Separators = separators;
            Current = default;
        }

        public bool MoveNext()
        {
            if (Span.IsEmpty)
            {
                return false;
            }

            (int index, int separatorLength) = FindSeparatorInSpan();
            if (index == -1 || separatorLength == 0)
            {
                Current = Span;
                Span = default;
            }
            else
            {
                Current = Span.Slice(0, index);
                Span = Span.Slice(index + separatorLength);
                if (Current.IsEmpty)
                {
                    return MoveNext();
                }
            }

            return true;
        }

        private (int position, int separatorLength) FindSeparatorInSpan()
        {
            int firstOccurance = int.MaxValue;
            int separatorLength = 0;
            foreach (string separator in Separators)
            {
                int indexOfOccurance = Span.IndexOf(separator);
                if (indexOfOccurance != -1 && indexOfOccurance < firstOccurance)
                {
                    firstOccurance = indexOfOccurance;
                    separatorLength = separator.Length;
                }
            }

            return (firstOccurance, separatorLength);
        }
    }
}