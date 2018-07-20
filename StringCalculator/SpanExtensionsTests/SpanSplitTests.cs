using FluentAssertions;
using SpanExtensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace SpanExtensionsTests
{
    public class SpanSplitTests
    {
        [Theory]
        [InlineData("", new string[] { "" })]
        [InlineData("", new string[] { "a" })]
        [InlineData("a", new string[] { "" })]
        [InlineData("abacada", new string[] { "b", "c", "d" })]
        [InlineData("abacada", new string[] { "a", "b", "c", "d" })]
        [InlineData("abbaccadda", new string[] { "bb", "cc", "dd" })]
        [InlineData("bbaccadda", new string[] { "bb", "cc", "dd" })]
        [InlineData("abbaccadd", new string[] { "bb", "cc", "dd" })]
        public void SplitMultipleSeparator(string str, string[] separators)
        {
            var actualParts = new List<string>();

            foreach (ReadOnlySpan<char> part in str.AsSpan().Split(separators))
            {
                actualParts.Add(part.ToString());
            }

            var expected = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            actualParts.Should().BeEquivalentTo(expected);
        }


        [Theory]
        [InlineData("", "")]
        [InlineData("", "a")]
        [InlineData("a", "")]
        [InlineData("a", "a")]
        [InlineData("aaaa", "a")]
        [InlineData("bab", "a")]
        [InlineData("abababab", "a")]
        [InlineData("babababa", "a")]
        [InlineData("aaaaab", "a")]
        [InlineData("baaaaa", "a")]
        [InlineData("zzzzzazzzzazzzzazzz", "a")]
        [InlineData("abba", "bb")]
        [InlineData("bbb", "bb")]
        [InlineData("bbaaabbaaabbaaabb", "bb")]
        public void SplitSingleSeparator(string str, string separator)
        {
            var actualParts = new List<string>();

            foreach (ReadOnlySpan<char> part in str.AsSpan().Split(separator))
            {
                actualParts.Add(part.ToString());
            }

            var expected = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            actualParts.Should().BeEquivalentTo(expected);
        }
    }
}
