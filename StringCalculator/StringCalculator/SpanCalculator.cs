using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculators
{
    public class SpanCalculator : IStringCalculator
    {
        public string Add(string input)
        {
            (string[] separators, string expression) = ParseToSeparatorsAndExpression(input.AsSpan());

            var numbers = GetNumbers(separators, expression);

            ValidateNoNegativeNumbers(numbers);

            numbers = FilterNumbersBiggerThan1000(numbers);

            return numbers.Sum().ToString();
        }

        private IEnumerable<int> FilterNumbersBiggerThan1000(IEnumerable<int> numbers)
            => numbers.Where(n => n <= 1000);

        private static IEnumerable<int> GetNumbers(string[] separators, string expression)
        {
            return expression.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));
        }

        private void ValidateNoNegativeNumbers(IEnumerable<int> numbers)
        {
            var negativeNumbers = numbers.Where(n => n < 0);
            if (negativeNumbers.Any())
            {
                throw new ApplicationException($"Input has the following negative numbers: {string.Join(",", negativeNumbers)}. Negative numbers are now allowed.");
            }
        }

        private static (string[] separators, string expression) ParseToSeparatorsAndExpression(ReadOnlySpan<char> expression)
        {
            var separators = DefaultSeparators();
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

        private static string[] DefaultSeparators()
        {
            return new string[] { ",", "\n" };
        }
    }
}
