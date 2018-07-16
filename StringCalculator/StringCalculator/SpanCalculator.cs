using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculators
{
    public class SpanCalculator : IStringCalculator
    {
        static readonly string[] DefaultSeparators = new string[] { ",", "\n" };

        public string Add(in ReadOnlySpan<char> input)
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

        private Span<int> FilterNumbersBiggerThan1000(int[] numbers)
        {
            Array.Sort(numbers);
            int index = 0;
            while (index < numbers.Length && numbers[index] <= 1000)
            {
                index++;
            }

            return numbers.AsSpan().Slice(0, index);
        }

        private static int[] GetNumbers(string[] separators, string expression)
        {
            return expression.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.AsSpan())).ToArray();
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
