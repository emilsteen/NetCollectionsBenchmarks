// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

using BenchmarkDotNet.Running;
using NetCollectionsBenchmarks;


BenchmarkRunner.Run<CollectionsBenchmarks>();

#if false // For Profiling
CollectionsBenchmarks bench = new CollectionsBenchmarks();
bench.Setup();

Console.WriteLine("Press Enter to start benchmark.");
Console.ReadLine();
var timer = Stopwatch.StartNew();
bench.ForeachNoTryGetValueSortedListBenchmark();
// bench.ForeachNoTryGetValueDictionaryBenchmark();

Console.WriteLine("Benchmark took: " + timer.ElapsedMilliseconds + " ms.");
#endif