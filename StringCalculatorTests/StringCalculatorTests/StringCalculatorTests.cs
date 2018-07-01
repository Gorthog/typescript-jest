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
    }

    internal class StringCalculator
    {
        internal string Add(string input)
        {
            (char[] separators, string expression) = ParseToSeparatorsAndExpression(input);

            var numbers = GetNumbers(separators, expression);

            ValidateNoNegativeNumbers(numbers);

            return numbers.Sum().ToString();
        }

        private static IEnumerable<int> GetNumbers(char[] separators, string expression)
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

        private static (char[] separators, string expression) ParseToSeparatorsAndExpression(string expression)
        {
            char[] separators = DefaultSeparators();
            if (AreOptionalSeparatorsSpecified(expression))
            {
                separators = ExtractOptionalSeparators(expression);
                expression = expression.Substring(3);
            }

            return (separators, expression);
        }

        private static char[] ExtractOptionalSeparators(string expression)
        {
            char[] separators = new char[] { expression.Split("\n", StringSplitOptions.RemoveEmptyEntries)[0].ToCharArray()[2] };
            return separators;
        }

        private static bool AreOptionalSeparatorsSpecified(string expression)
        {
            return expression.StartsWith("//");
        }

        private static char[] DefaultSeparators()
        {
            return new char[] { ',', '\n' };
        }
    }
}
