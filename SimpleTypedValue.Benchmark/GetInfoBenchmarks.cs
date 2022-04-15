using BenchmarkDotNet.Attributes;

namespace SimpleTypedValue.Benchmark;

public class GetInfoBenchmarks
{
    [Benchmark(Baseline = true)]
    public TypedValueInfo Test_Info()
    {
        return TypedValue<TestId>.Info;
    }

    [Benchmark]
    public TypedValueInfo Test_GetInfoGeneric()
    {
        return TypedValue.GetInfo<TestId>()!;
    }

    [Benchmark]
    public TypedValueInfo Test_GetInfo()
    {
        return TypedValue.GetInfo(typeof(TestId))!;
    }

    [Benchmark]
    public TypedValueInfo? Test_GetInfoLong()
    {
        return TypedValue.GetInfo(typeof(long))!;
    }

    public struct TestId : ITypedValue<long>
    {
        public TestId()
        {
            Value = default;
        }

        public TestId(long id)
        {
            Value = id;
        }

        public long Value { get; }
    }
}