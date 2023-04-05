using BenchmarkDotNet.Running;
using GraphCW.Benchmarks;

var summary = BenchmarkRunner.Run(typeof(PathFinderBenchmarks).Assembly);
