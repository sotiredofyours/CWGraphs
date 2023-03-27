using BenchmarkDotNet.Attributes;

namespace GraphCW.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class PathFinderBenchmarks
{
    private Graph graph = GraphGenerator.GenerateGraph(200, 200);
    
    [Benchmark]
    public void MultiThreadTest()
    {
        PathFinder.FindLongestPath(graph);
    }
    
    [Benchmark]
    public void SingleThreadTest()
    {
        PathFinder.FindLongestPathSingle(graph);
    }
}