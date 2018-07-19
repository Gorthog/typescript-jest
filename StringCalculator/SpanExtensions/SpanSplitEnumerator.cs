using System;

namespace SpanExtensions
{
    public ref struct SpanSplitEnumerator
    {
        ReadOnlySpan<char> Span { get; set; }
        ReadOnlySpan<char> Separator { get; }
        public ReadOnlySpan<char> Current { get; private set; }

        public SpanSplitEnumerator(in ReadOnlySpan<char> span, in ReadOnlySpan<char> separator)
        {
            Span = span;
            Separator = separator;
            Current = default;
        }

        public bool MoveNext()
        {
            if (Span.IsEmpty)
            {
                return false;
            }

            int index = Span.IndexOf(Separator);
            if (index == -1 || Separator.IsEmpty)
            {
                Current = Span;
                Span = default;
            }
            else
            {
                Current = Span.Slice(0, index);
                Span = Span.Slice(index + Separator.Length);
                if (Current.IsEmpty)
                {
                    return MoveNext();
                }
            }

            return true;
        }


    }
}