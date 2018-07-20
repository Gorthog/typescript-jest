using SpanExtensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculators
{
    public class SpanCalculator : IStringCalculator
    {
        ArrayPool<int> IntPool = ArrayPool<int>.Shared;
        int[] IntBuffer;
        static readonly string[] DefaultSeparators = new string[] { ",", "\n" };

        public string Add(in ReadOnlySpan<char> input)
        {
            try
            {
                (string[] separators, string expression) = ParseToSeparatorsAndExpression(input);

                var numbers = GetNumbers(separators, expression);

                ValidateNoNegativeNumbers(numbers);

                var filteredNumbers = FilterNumbersBiggerThan1000(numbers);

                var sum = 0;
                foreach (var number in filteredNumbers)
                {
                    sum += number;
                }

                return sum.ToString();
            }
            finally
            {
                IntPool.Return(IntBuffer);
            }
        }

        private Span<int> FilterNumbersBiggerThan1000(in Span<int> numbers)
        {
            int indexOfNumberSmallerThan1000 = 0;
            while (indexOfNumberSmallerThan1000 < numbers.Length && numbers[indexOfNumberSmallerThan1000] > 1000)
            {
                indexOfNumberSmallerThan1000++;
            }

            if (indexOfNumberSmallerThan1000 == numbers.Length)
            {
                return Span<int>.Empty;
            }

            int index = 0;
            for (int i = indexOfNumberSmallerThan1000; i < numbers.Length; i++)
            {
                if (numbers[i] > 1000)
                {
                    (numbers[i], numbers[indexOfNumberSmallerThan1000]) = (numbers[indexOfNumberSmallerThan1000], numbers[i]);
                    indexOfNumberSmallerThan1000++;
                }
            }

            return numbers.Slice(indexOfNumberSmallerThan1000);
        }

        public static void GetNumbersVoid(string[] separators, ReadOnlySpan<char> expression)
        {
            var list = new LinkedList<int>();
            foreach (var number in expression.Split(separators))
            {
                list.AddLast(int.Parse(number));
            }

            list.ToArray();
        }

        public Span<int> GetNumbers(string[] separators, ReadOnlySpan<char> expression)
        {
            IntBuffer = IntPool.Rent(100);
            int index = 0;
            foreach (var number in expression.Split(separators))
            {
                if (index == IntBuffer.Length)
                {
                    throw new Exception("buffer too small");
                }

                IntBuffer[index] = int.Parse(number);
                index++;
            }

            return IntBuffer.AsSpan(0, index);
        }

        private void ValidateNoNegativeNumbers(Span<int> numbers)
        {
            Span<int> negativeNumbers = stackalloc int[numbers.Length];
            int index = 0;
            foreach (var number in numbers)
            {
                if (number < 0)
                {
                    negativeNumbers[index] = number;
                    index++;
                }

            }

            if (index > 0)
            {
                throw new ApplicationException($"Input has the following negative numbers: {string.Join(",", negativeNumbers.ToArray())}. Negative numbers are not allowed.");
            }
        }

        private static (string[] separators, string expression) ParseToSeparatorsAndExpression(ReadOnlySpan<char> expression)
        {
            var separators = DefaultSeparators;
            if (AreOptionalSeparatorsSpecified(expression))
            {
                expression = expression.Slice(2);
                var splitPosition = expression.IndexOf('\n');
                separators = ExtractOptionalSeparators(expression.Slice(0, splitPosition));
                expression = expression.Slice(splitPosition);
            }

            return (separators, expression.ToString());
        }

        private static string[] ExtractOptionalSeparators(ReadOnlySpan<char> header)
        {
            if (header.Length == 1)
            {
                return new string[] { header.ToString() };
            }

            var separators = new LinkedList<string>();
            int separatorStartIndex = 0, separatorLength = 0;
            while (separatorStartIndex < header.Length && header[separatorStartIndex] == '[')
            {
                separatorStartIndex++;
                while (header[separatorStartIndex + separatorLength] != ']')
                {
                    separatorLength++;
                }

                separators.AddLast(header.Slice(separatorStartIndex, separatorLength).ToString());
                separatorStartIndex += separatorLength + 1;
                separatorLength = 0;
            }

            return separators.ToArray();
        }

        private static bool AreOptionalSeparatorsSpecified(ReadOnlySpan<char> expression)
        {
            return expression.StartsWith("//");
        }

        public string Add(string input)
        {
            return Add(input.AsSpan());
        }
    }
}
