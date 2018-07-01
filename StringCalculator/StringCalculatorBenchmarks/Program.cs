using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using StringCalculators;

namespace StringCalculatorBenchmarks
{
    [MemoryDiagnoser]
    public class StringCalculators
    {
        StringCalculator stringCalculator = new StringCalculator();
        SpanCalculator spanCalculator = new SpanCalculator();

        [Benchmark(Baseline = true)]
        public string StringCalculator() => stringCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4");

        [Benchmark]
        public string SpanCalculator() => spanCalculator.Add("//[***][%%%][^^]\n1***2%%%3^^4");
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StringCalculators>();
        }
    }
}
