using System;

namespace SpanExtensions
{
    public static class SpanFilterExtensions
    {
        public static Span<T> Filter<T>(this Span<T> span, Predicate<T> filter)
        {
            int indexOfFirstNotFilteredItem = FindFirstOccuranceOfNotFilteredItem(span, filter);

            indexOfFirstNotFilteredItem = SwapFilteredItemsToSpanStart(ref span, filter, indexOfFirstNotFilteredItem);

            return span.Slice(indexOfFirstNotFilteredItem);
        }

        private static int FindFirstOccuranceOfNotFilteredItem<T>(in Span<T> span, Predicate<T> predicate)
        {
            int indexOfFirstNotFilteredItem = 0;
            while (indexOfFirstNotFilteredItem < span.Length && predicate(span[indexOfFirstNotFilteredItem]))
            {
                indexOfFirstNotFilteredItem++;
            }

            return indexOfFirstNotFilteredItem;
        }

        private static int SwapFilteredItemsToSpanStart<T>(ref Span<T> span, Predicate<T> filter, int indexOfFirstNotFilteredItem)
        {
            for (int i = indexOfFirstNotFilteredItem + 1; i < span.Length; i++)
            {
                if (filter(span[i]))
                {
                    (span[i], span[indexOfFirstNotFilteredItem]) = (span[indexOfFirstNotFilteredItem], span[i]);
                    indexOfFirstNotFilteredItem++;
                }
            }

            return indexOfFirstNotFilteredItem;
        }
    }
}
