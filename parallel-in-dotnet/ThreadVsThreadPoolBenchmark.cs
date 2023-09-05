using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Bnaya.Samples;

[SimpleJob(RuntimeMoniker.Net80)]
[RPlotExporter]
[GcServer(true)]
public class ThreadVsThreadPoolBenchmark
{
    [Params(100)]
    public int JobCount;

    [Params(10_000)]
    public int Iterations;

    [Benchmark(Baseline = true)]
    public int Synchronic() => SynchronicBenchmark.Compute(JobCount, Iterations);

    [Benchmark]
    public int Thread() => ThreadBenchmark.Compute(JobCount, Iterations);

    [Benchmark]
    public int Pool() => ThreadBenchmark.Compute(JobCount, Iterations);

    [Benchmark]
    public int PLinq() => PLinqBenchmark.Compute(JobCount, Iterations);
}
