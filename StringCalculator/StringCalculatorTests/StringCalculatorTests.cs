using FluentAssertions;
using StringCalculators;
using System;
using Xunit;

// http://osherove.com/tdd-kata-1/

namespace StringCalculatorTests
{

    public class StringCalculatorTests
    {
        IStringCalculator stringCalculator = new SpanCalculator();

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
            Action act = () => 
                stringCalculator.Add("-1,5,-3");

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
}
