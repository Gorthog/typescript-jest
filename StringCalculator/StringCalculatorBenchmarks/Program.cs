using BytesUtilities;
using StringCalculators;
using System;

namespace StringCalculatorBenchmarks
{
    public class Calculators
    {
        StringCalculator stringCalculator = new StringCalculator();
        SpanCalculator spanCalculator = new SpanCalculator();

        public void StringCalculator() => stringCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4");

        public void SpanCalculator() => spanCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4");

        string[] separatos = new[] { "***", "%%%", "^^" };
        string expression = "\n1***2%%%3^^4";

        public void GetNumbers() => spanCalculator.GetNumbers(separatos, expression);
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
            var calculators = new Calculators();
            // warmup 
            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: calculators.StringCalculator);
            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: calculators.SpanCalculator);


            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: calculators.StringCalculator);
            allocationStats.PrintTotalAllocatedBytes(actionToBeMeasured: calculators.SpanCalculator);
            allocationStats.PrintTotalAllocatedBytes(() => calculators.GetNumbers());
        }
    }
}
