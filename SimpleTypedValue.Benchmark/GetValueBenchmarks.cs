using BenchmarkDotNet.Attributes;

namespace SimpleTypedValue.Benchmark;

public class GetValueBenchmarks
{
    private readonly TestId _testValue = new(100);
    private readonly ITypedValue _testValueObject = new TestId(100);
    private readonly ITypedValue<long> _testValueLongObject = new TestId(100);

    [Benchmark(Baseline = true)]
    public long GetValue()
    {
        return _testValue.Value;
    }

    [Benchmark]
    public object? GetValue_ITypedValue()
    {
        return _testValueObject.Value;
    }

    [Benchmark]
    public long? GetValue_ITypedValueGeneric()
    {
        return _testValueLongObject.Value;
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

        public TestId From(long value)
        {
            return new TestId(value);
        }
    }
}