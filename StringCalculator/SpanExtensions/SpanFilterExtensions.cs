using System;

namespace SpanExtensions
{
    public static class SpanFilterExtensions
    {
        public static Span<T> Filter<T>(this Span<T> span, Predicate<T> predicate)
        {
            int indexOfNotFilteredItem = FindFirstOccuranceOfNotFilteredItem(span, predicate);

            if (indexOfNotFilteredItem == span.Length)
            {
                return Span<T>.Empty;
            }

            indexOfNotFilteredItem = MoveAllFilteredItemsToSpanStart(ref span, predicate, indexOfNotFilteredItem);

            return span.Slice(indexOfNotFilteredItem);
        }

        private static int MoveAllFilteredItemsToSpanStart<T>(ref Span<T> span, Predicate<T> predicate, int indexOfNotFilteredItem)
        {
            for (int i = indexOfNotFilteredItem + 1; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    (span[i], span[indexOfNotFilteredItem]) = (span[indexOfNotFilteredItem], span[i]);
                    indexOfNotFilteredItem++;
                }
            }

            return indexOfNotFilteredItem;
        }

        private static int FindFirstOccuranceOfNotFilteredItem<T>(in Span<T> span, Predicate<T> predicate)
        {
            int indexOfNotFilteredItem = 0;
            while (indexOfNotFilteredItem < span.Length && predicate(span[indexOfNotFilteredItem]))
            {
                indexOfNotFilteredItem++;
            }

            return indexOfNotFilteredItem;
        }
    }
}
