using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

// http://osherove.com/tdd-kata-1/

namespace StringCalculatorTests
{

    public class StringCalculatorTests
    {
        StringCalculator stringCalculator = new StringCalculator();

        [Fact]
        void Empty()
        {
            var result = stringCalculator.Add("");

            result.Should().Be("0");

        }

        [Theory]
        [InlineData("1")]
        [InlineData("7")]
        [InlineData("57")]
        void OneNumberReturnsTheSame(string stringExpression)
        {
            var result = stringCalculator.Add(stringExpression);

            result.Should().Be(stringExpression);
        }


        [Theory]
        [InlineData("1,2", "3")]
        [InlineData("4,4", "8")]
        [InlineData("40,51", "91")]
        [InlineData("7,8,1,1,1,1", "19")]
        [InlineData("456,391", "847")]
        [InlineData("1000,2", "1002")]

        void ReturnsSumWithCommaSeparator(string stringExpression, string expected)
        {
            var result = stringCalculator.Add(stringExpression);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("1\n2", "3")]
        [InlineData("456\n391,5,10\n0", "862")]
        void ReturnsSumWithNewlineSeparator(string stringExpression, string expected)
        {
            var result = stringCalculator.Add(stringExpression);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("//;\n1;2", "3")]
        void OptionalSeparator(string stringExpression, string expected)
        {
            var result = stringCalculator.Add(stringExpression);

            result.Should().Be(expected);
        }

        [Fact]
        void NegativeNumbersThrowExpection()
        {
            Action act = () => stringCalculator.Add("-1,5,-3");

            act.Should()
               .Throw<ApplicationException>()
               .WithMessage("*-1,-3*");
        }

        [Fact]
        void IgnoreNumbersBiggerThan1000()
        {
            var result = stringCalculator.Add("2,1001");

            result.Should().Be("2");
        }

        [Fact]
        void AllowDelimtersOfVariableLength()
        {
            var result = stringCalculator.Add("//[***]\n1***2***3");

            result.Should().Be("6");
        }

        [Theory]
        [InlineData("//[*][%]\n1*2%3", "6")]
        [InlineData("//[***][%%%][^^]\n1***2%%%3^^4", "10")]
        void AllowMultipleDelimetersOfVariableLength(string stringExpression, string expected)
        {
            var result = stringCalculator.Add(stringExpression);

            result.Should().Be(expected);
        }
    }

    internal class StringCalculator
    {
        internal string Add(string input)
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
    }
}
