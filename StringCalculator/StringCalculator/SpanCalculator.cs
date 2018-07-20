using SpanExtensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculators
{
    public class SpanCalculator : IStringCalculator
    {
        internal ref struct SpanHolder
        {
            public SpanHolder(Span<string> separators, ReadOnlySpan<char> expression) : this()
            {
                Separators = separators;
                Expression = expression;
            }

            internal Span<string> Separators { get; set; }
            internal ReadOnlySpan<char> Expression { get; set; }
        }

        ArrayPool<int> IntPool = ArrayPool<int>.Shared;
        ArrayPool<string> StringPool = ArrayPool<string>.Shared;
        int[] IntBuffer;
        string[] StringBuffer;
        static readonly string[] DefaultSeparators = new string[] { ",", "\n" };

        public string Add(string input)
        {
            return Add(input.AsSpan());
        }

        public string Add(in ReadOnlySpan<char> input)
        {
            try
            {
                var spanHolder = ParseToSeparatorsAndExpression(input);

                var numbers = GetNumbers(spanHolder.Separators, spanHolder.Expression);

                ValidateNoNegativeNumbers(numbers);

                var filteredNumbers = numbers.Filter(n => n > 1000);

                var sum = 0;
                foreach (var number in filteredNumbers)
                {
                    sum += number;
                }

                return sum.ToString();
            }
            finally
            {
                if (IntBuffer != null)
                {
                    IntPool.Return(IntBuffer);
                }

                if (StringBuffer != null)
                {
                    StringPool.Return(StringBuffer);
                }
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

        public Span<int> GetNumbers(Span<string> separators, ReadOnlySpan<char> expression)
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

        private SpanHolder ParseToSeparatorsAndExpression(ReadOnlySpan<char> expression)
        {
            Span<string> separators = DefaultSeparators;
            if (AreOptionalSeparatorsSpecified(expression))
            {
                expression = expression.Slice(2);
                var splitPosition = expression.IndexOf('\n');
                separators = ExtractOptionalSeparators(expression.Slice(0, splitPosition));
                expression = expression.Slice(splitPosition);
            }

            return new SpanHolder(separators, expression);
        }

        private Span<string> ExtractOptionalSeparators(in ReadOnlySpan<char> header)
        {
            StringBuffer = StringPool.Rent(100);
            if (header.Length == 1)
            {
                StringBuffer[0] = header.ToString();
                return StringBuffer.AsSpan(0, 1);
            }

            Span<string> separators = StringBuffer;
            int separatorStartIndex = 0, separatorLength = 0, separatorsCount = 0;
            while (separatorStartIndex < header.Length && header[separatorStartIndex] == '[')
            {
                separatorStartIndex++;
                while (header[separatorStartIndex + separatorLength] != ']')
                {
                    separatorLength++;
                }

                separators[separatorsCount++] = header.Slice(separatorStartIndex, separatorLength).ToString();
                separatorStartIndex += separatorLength + 1;
                separatorLength = 0;
            }

            return separators.Slice(0, separatorsCount);
        }

        private static bool AreOptionalSeparatorsSpecified(ReadOnlySpan<char> expression)
        {
            return expression.StartsWith("//");
        }
    }
}
