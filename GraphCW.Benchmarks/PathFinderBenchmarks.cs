using BenchmarkDotNet.Attributes;

namespace GraphCW.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class PathFinderBenchmarks
{
    private readonly Graph _graph = GraphGenerator.GenerateGraph(30, 100);
    
    [Benchmark]
    public void MultiThreadTest()
    {
        PathFinder.FindLongestPath(_graph);
    }
    
    [Benchmark]
    public void SingleThreadTest()
    {
        PathFinder.FindLongestPathSingle(_graph);
    }
}