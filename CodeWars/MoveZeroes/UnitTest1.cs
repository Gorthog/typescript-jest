using FluentAssertions;
using System.Linq;
using Xunit;

namespace MoveZeroes
{
    // https://www.codewars.com/kata/52597aa56021e91c93000cb0/train/csharp
    public class Kata
    {
        public static int[] MoveZeroes(int[] arr)
        {
            return arr.Where(i => i != 0).Concat(arr.Where(i => i == 0)).ToArray();
        }
    }

    public class UnitTest1
    {
        public UnitTest1()
        {
            AssertionOptions.AssertEquivalencyUsing(options => options.WithStrictOrdering());
        }

        [Fact]
        public void TestNoZeroes()
            => Kata.MoveZeroes(new[] { 1, 2, 3 }).Should().BeEquivalentTo(new[] { 1, 2, 3 });

        [Fact]
        public void OneZero()
            => Kata.MoveZeroes(new[] { 0, 1, 2, 3 }).Should().BeEquivalentTo(new[] { 1, 2, 3, 0 });

        [Fact]
        public void MultipleZeroes()
        {
            Kata.MoveZeroes(new int[] { 1, 2, 0, 1, 0, 1, 0, 3, 0, 1 }).Should().BeEquivalentTo(new int[] { 1, 2, 1, 1, 3, 1, 0, 0, 0, 0 });
        }
    }
}
