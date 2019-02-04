using FluentAssertions;
using FluentAssertions.Extensions;
using System;
using Xunit;

namespace BouncingBall
{
    // https://www.codewars.com/kata/5544c7a5cb454edb3c000047/train/csharp
    public class BouncingBall
    {

        public static int bouncingBall(double h, double bounce, double window)
        {
            if (h <= 0 ||
                bounce <= 0 || bounce >= 1 ||
                window >= h)
            {
                return -1;
            }

            var bounces = (int)System.Math.Log(window / h, bounce);

            return 1 + 2 * bounces;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void TestInvalidInputH()
        {
            BouncingBall.bouncingBall(0, 0.66, -1).Should().Be(-1);
        }

        [Fact]
        public void TestInvalidInputBounceLessThan0()
        {
            BouncingBall.bouncingBall(4, -1, 1.5).Should().Be(-1);
        }

        [Fact]
        public void TestInvalidInputBounceMoreThan1()
        {
            BouncingBall.bouncingBall(4, 2, 1.5).Should().Be(-1);
        }

        [Fact]
        public void TestInvalidInputWindowsMoreThanH()
        {
            BouncingBall.bouncingBall(1.5, 2, 1.5).Should().Be(-1);
        }

        
        [Fact]
        public void Test1()
        {
            BouncingBall.bouncingBall(3.0, 0.66, 1.5).Should().Be(3);
        }


        [Fact]
        public void Test2()
        {
            BouncingBall.bouncingBall(30.0, 0.66, 1.5).Should().Be(15);
        }

        [Fact]
        public void Performance()
        {
            Action longExecution = () => BouncingBall.bouncingBall(100, 0.999999999999, 1);
            longExecution.ExecutionTime().Should().BeLessOrEqualTo(20.Milliseconds());
        }
    }
}
