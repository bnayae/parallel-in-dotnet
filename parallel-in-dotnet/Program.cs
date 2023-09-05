// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

using BenchmarkDotNet.Running;

using Bnaya.Samples;


Console.WriteLine("Start benchmark!");

int jobCount = 10_000;
int iterations = 10_001;

var sw = Stopwatch.StartNew();
var r = SynchronicBenchmark.Compute(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Synchronic [{r:N0}]: {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = ThreadBenchmark.Compute(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Thread [{r:N0}]:           {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = ThreadPoolBenchmark.Compute(jobCount, iterations);
sw.Stop();
Console.WriteLine($"ThreadPool [{r:N0}]:       {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = PLinqBenchmark.Compute(jobCount, iterations);
sw.Stop();
Console.WriteLine($"PLinq [{r:N0}]:            {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await TaskBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Task [{r:N0}]:             {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await AsyncBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Async [{r:N0}]:            {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await DataflowBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Dataflow [{r:N0}]:         {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await DataflowSimpleBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Dataflow Simple [{r:N0}]:  {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await ChannelBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Channel Simple [{r:N0}]:   {sw.ElapsedMilliseconds / 1000.0:N3}");

sw = Stopwatch.StartNew();
r = await ChannelMultiReadBenchmark.ComputeAsync(jobCount, iterations);
sw.Stop();
Console.WriteLine($"Channel Multi [{r:N0}]:    {sw.ElapsedMilliseconds / 1000.0:N3}");

Console.ReadLine();


var summary = BenchmarkRunner.Run<ThreadVsThreadPoolBenchmark>();
Console.WriteLine("================================================");
Console.WriteLine(summary);
Console.WriteLine("Benchmark complete");
Console.ReadLine();
