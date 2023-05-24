using BenchmarkDotNet.Attributes;

namespace GraphCW.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class PathFinderBenchmarks
{
    [ParamsSource(nameof(ValuesForGraph))]
    public Graph _graph;
    public static IEnumerable<Graph> ValuesForGraph() => new[]
    {
        GraphGenerator.GenerateGraph(10, 20), 
        GraphGenerator.GenerateGraph(15, 30), 
        GraphGenerator.GenerateGraph(20, 40),
        GraphGenerator.GenerateGraph(25, 50),
    };

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
