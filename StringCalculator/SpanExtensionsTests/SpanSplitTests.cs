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
        public void Split(string str, string separator)
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
