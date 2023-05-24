namespace GraphCW;

public static class GraphGenerator
{
    public static Graph GenerateGraph(int nodes, int edges)
    {
        var rnd = new Random();
        var graph = new Graph(nodes);
        for (int i = 0; i < edges; i++)
        {
            var from = rnd.Next(nodes - 1);
            var to = rnd.Next(nodes - 1);
            while (graph.IsConnected(graph[from], graph[to]) || from == to)
            {
                to = rnd.Next(nodes - 1);
                from = rnd.Next(nodes - 1);
            }
            graph.Connect(to, from);
        }
        return graph;
    }
}