using System;
using System.Collections.Generic;
using System.Linq;

namespace StringCalculators
{
    public class StringCalculator : IStringCalculator
    {
        public string Add(string input)
        {
            (string[] separators, string expression) = ParseToSeparatorsAndExpression(input);

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

        private static (string[] separators, string expression) ParseToSeparatorsAndExpression(string expression)
        {
            var separators = DefaultSeparators();
            if (AreOptionalSeparatorsSpecified(expression))
            {
                expression = expression.Substring(2);
                var splitExpressions = expression.Split("\n", 2, StringSplitOptions.RemoveEmptyEntries);
                separators = ExtractOptionalSeparators(splitExpressions[0]);
                expression = splitExpressions[1];
            }

            return (separators, expression);
        }

        private static string[] ExtractOptionalSeparators(string header)
        {
            if (header.Length == 1)
            {
                return new string[] { header };
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

                separators.AddLast(header.Substring(separatorStartIndex, separatorLength));
                separatorStartIndex += separatorLength + 1;
                separatorLength = 0;
            }

            return separators.ToArray();
        }

        private static bool AreOptionalSeparatorsSpecified(string expression)
        {
            return expression.StartsWith("//");
        }

        private static string[] DefaultSeparators()
        {
            return new string[] { ",", "\n" };
        }

        public string Add(in ReadOnlySpan<char> input)
        {
            return Add(input.ToString());
        }
    }
}
