using FluentAssertions;
using System;
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
    }

    internal class StringCalculator
    {
        internal string Add(string expression)
        {
            char[] separators = new char[] { ',', '\n' };
            if (expression.StartsWith("//"))
            {
                separators = new char[] { expression.Split("\n", StringSplitOptions.RemoveEmptyEntries)[0].ToCharArray()[2] };
                expression = expression.Substring(3);
            }

            var numbers = expression.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (!numbers.Any())
            {
                return "0";
            }

            return numbers.Select(n => int.Parse(n)).Sum().ToString();
        }
    }
}
