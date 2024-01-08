namespace GraphCW;

public static class GraphCreator
{
    public static Graph CreateGraph(int nodes, IEnumerable<string> edgesCommands)
    {
        var graph = new Graph(nodes);
        foreach (var con in edgesCommands)
        {
            var (from, to) = ParseConnection(con);
            graph.Connect(from, to);
        }
        return graph;
    }
    
    private static (int, int) ParseConnection(string connection)
    {
        var parts = connection.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3) throw new ArgumentException();
        return (Convert.ToInt32(parts[0]), Convert.ToInt32(parts[2]));
    }
}