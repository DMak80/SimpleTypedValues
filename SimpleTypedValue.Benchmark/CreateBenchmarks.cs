using BenchmarkDotNet.Attributes;

namespace SimpleTypedValue.Benchmark;

public class CreateBenchmarks
{
    private readonly TypedValueInfo Info = TypedValue.GetInfo<TestId>()!;

    [Benchmark(Baseline = true)]
    public TestId Test_New()
    {
        return new TestId(100);
    }

    [Benchmark]
    public TestId TestCreate_LongViaCreatorShort()
    {
        return TypedValueCreator<TestId>.Create(100);
    }

    [Benchmark]
    public TestId TestCreate_LongViaCreator()
    {
        return TypedValueCreator<TestId, long>.Create(100);
    }

    [Benchmark]
    public TestId TestCreate_IntViaCreatorShort()
    {
        return TypedValueCreator<TestId>.Create(100);
    }

    [Benchmark]
    public TestId TestCreate_IntViaCreator()
    {
        return TypedValueCreator<TestId, int>.Create(100);
    }

    public readonly struct TestId : ITypedValue<long>
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