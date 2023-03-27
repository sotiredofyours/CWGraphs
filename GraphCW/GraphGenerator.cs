namespace GraphCW;

public static class GraphGenerator
{
    public static Graph GenerateGraph(int minNodes, int maxNodes)
    {
        var rnd = new Random();
        var nodes = rnd.Next(minNodes, maxNodes);
        var graph = new Graph(nodes);
        var edges = rnd.Next(nodes, nodes * (nodes - 1) / 2);
        for (int i = 0; i < edges; i++)
        {
            var from = rnd.Next(nodes - 1);
            var to = rnd.Next(nodes - 1);
            while (graph.IsConnected(graph[from], graph[to]) || from == to)
            {
                to = rnd.Next(nodes - 1);
            }
            graph.Connect(to, from);
        }
        return graph;
    }
}