using FluentAssertions;
using SpanExtensions;
using System;
using Xunit;

namespace SpanExtensionsTests
{
    public class SpanFilterTests
    {
        [Fact]
        void ItFiltersEmptyArray()
        {
            Span<int> span = Span<int>.Empty;
            var actual = span.Filter(x => x > 1);
            actual.ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        }

        [Fact]
        void ItFiltersLastElement()
        {
            Span<int> span = new[] { 1, 2 };
            var actual = span.Filter(x => x > 1);
            actual.ToArray().Should().BeEquivalentTo(new[] { 1 });

        }

        [Fact]
        void ItFiltersFirstElement()
        {
            Span<int> span = new[] { 1, 2 };
            var actual = span.Filter(x => x == 1);
            actual.ToArray().Should().BeEquivalentTo(new[] { 2 });
        }

        [Fact]
        void ItFiltersElementInTheMiddle()
        {
            Span<int> span = new[] { 1, 2, 1 };
            var actual = span.Filter(x => x == 2);
            actual.ToArray().Should().BeEquivalentTo(new[] { 1, 1 });
        }

        [Fact]
        void ItFiltersAllElements()
        {
            Span<int> span = new[] { 1, 1, 1 };
            var actual = span.Filter(x => true);
            actual.ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        }
    }
}
