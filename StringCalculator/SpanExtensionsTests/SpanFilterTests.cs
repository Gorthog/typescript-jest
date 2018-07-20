using FluentAssertions;
using SpanExtensions;
using System;
using Xunit;

namespace SpanExtensionsTests
{
    public class SpanFilterTests
    {
        [Fact]
        void ItExists()
        {
            Span<int> span = new[] { 1, 2 };
            span.Filter(x => x > 1000);
        }

        [Fact]
        void ItFiltersLastElement()
        {
            Span<int> span = new[] { 1, 2 };
            var actual = span.Filter(x => x > 1);
            actual.ToArray().Should().BeEquivalentTo(new[] { 1 });

        }

        [Fact]
        void ItFiltersEmptyArray()
        {
            Span<int> span = Span<int>.Empty;
            var actual = span.Filter(x => x > 1);
            actual.ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        }

        [Fact]
        void ItFiltersFirstElement()
        {
            Span<int> span = new[] { 1, 2 };
            var actual = span.Filter(x => x == 1);
            actual.ToArray().Should().BeEquivalentTo(new[] { 2 });
        }

        [Fact]
        void ItFiltersAll()
        {
            Span<int> span = new[] { 1, 1, 1 };
            var actual = span.Filter(x => true);
            actual.ToArray().Should().BeEquivalentTo(Array.Empty<int>());
        }
    }
}
