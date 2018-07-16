using BytesUtilities;
using StringCalculators;
using System;

namespace StringCalculatorBenchmarks
{
    public class StringCalculators
    {
        StringCalculator stringCalculator = new StringCalculator();
        SpanCalculator spanCalculator = new SpanCalculator();

        public void StringCalculator() => stringCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4");

        public void SpanCalculator() => spanCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4".AsSpan());
    }

    public class AllocationStats
    {
        HumanReadableBytesSize humanReadableBytesSize = new HumanReadableBytesSize();
        public void PrintTotalAllocatedBytes(Action actionToBeMeasured)
        {
            var allocatedBytes = MeasureAllocatedBytes(actionToBeMeasured);
            Console.WriteLine($"Number of bytes allocated: { humanReadableBytesSize.BytesToString(allocatedBytes)}");
        }

        public long MeasureAllocatedBytes(Action actionToBeMeasured)
        {
            var before = GC.GetAllocatedBytesForCurrentThread();
            actionToBeMeasured();
            return GC.GetAllocatedBytesForCurrentThread() - before;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var allocationStats = new AllocationStats();
            var stringCalculators = new StringCalculators();
            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: stringCalculators.StringCalculator);
            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: stringCalculators.SpanCalculator);
        }
    }
}
