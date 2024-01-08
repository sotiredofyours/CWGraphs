using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphCW;

namespace CWGraphs.Client.ViewModels;

public class GraphViewModel
{
    private Dictionary<int, int> _dict;
    public List<EdgeViewModel>? Edges { get; set; }
    public List<NodeViewModel>? Nodes { get; set; }

    public GraphViewModel(List<EdgeViewModel>? edges, List<NodeViewModel>? nodes)
    {
        Edges = edges;
        Nodes = nodes;
    }
    public async Task<List<int>?> CalculatePath()
    {
        if (Edges.Count is 0 || Nodes.Count is 0) return null;
        _dict = Nodes
            .OrderBy(x => x.Number)
            .Select((node, num) => new
            {
                Number = node.Number, Index = num
            })
            .ToDictionary(item => item.Number, item => item.Index);
        var graph = new Graph(Nodes.Count);
        foreach (var edge in Edges)
        {
            var fi = edge.Node1.Number;
            var si = edge.Node2.Number;
            graph.Connect(_dict[fi], _dict[si]);
        }

        var task = Task.Run(() =>
            PathFinder.FindLongestPath(graph)
        );

        var result = await task;
        var realResult = result?.Select(node => _dict.FirstOrDefault(x => x.Value == node.NodeNumber).Key).ToList();
        return realResult?.ToList();
    }
}