using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ValidBracesTests
{
    public class Brace
    {

        public static bool validBraces(string braces)
        {
            var stack = new Stack<char>();
            foreach (var brace in braces)
            {
                if (OpeningBrace(brace))
                {
                    stack.Push(brace);
                }
                else if (ClosingBrace(brace))
                {
                    var result = stack.TryPop(out var openningBrace);
                    if (!result || !MatchingBraces(openningBrace, brace))
                    {
                        return false;
                    }
                }
            }

            return !stack.Any();
        }

        private static bool MatchingBraces(char brace, char closingBrace)
        { 
            if ((brace, closingBrace) == ('{', '}') ||
                (brace, closingBrace) == ('(', ')') ||
                (brace, closingBrace) == ('[', ']'))
            {
                return true;
            }

            return false;
        }

        private static bool ClosingBrace(char brace)
        {
            if (brace == '}' ||
                brace == ']' ||
                brace == ')')
            {
                return true;
            }

            return false;
        }

        private static bool OpeningBrace(char brace)
        { 
            if (brace == '{' ||
                brace == '[' ||
                brace == '(')
            {
                return true;
            }

            return false;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
            => Brace.validBraces("()").Should().BeTrue();

        [Fact]
        public void Test2()
            => Brace.validBraces("[(])").Should().BeFalse();

        [Fact]
        public void Test3()
            => Brace.validBraces("))").Should().BeFalse();

        [Fact]
        public void Test4()
            => Brace.validBraces("((").Should().BeFalse();

        [Fact]
        public void Test5()
            => Brace.validBraces("({[[[{()}]]]})").Should().BeTrue();
    }
}
