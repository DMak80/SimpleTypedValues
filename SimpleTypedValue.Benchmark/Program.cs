using BenchmarkDotNet.Running;

namespace SimpleTypedValue.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<GetValueBenchmarks>();
        var summary2 = BenchmarkRunner.Run<CreateBenchmarks>();
        var summary3 = BenchmarkRunner.Run<GetInfoBenchmarks>();
    }
}