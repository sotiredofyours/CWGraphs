namespace GraphCW;

public static class PathFinder
{
    private static volatile int  _maxPath = 0;
    private static volatile int  _maxPathSingle = 0;
    private static object obj = new object();
    
    public static int FindLongestPath(Graph graph)
    {
        Parallel.ForEach(graph.Nodes, node => FindPath(node, 0, null, new List<Node>()));
        return _maxPath;
    }

    private static void FindPath(Node currentNode, int currentPath, Node previous, List<Node> used)
    {
        if (currentPath > _maxPath)
        { 
            _maxPath = currentPath;
        }
        used.Add(currentNode);
        var possiblePaths = currentNode.IncidentNodes.Where(x => x != previous && !used.Contains(x));
        foreach (var node in possiblePaths)
        { 
            FindPath(node, currentPath + 1, node, used);    
        }
    }

    public static int FindLongestPathSingle(Graph graph)
    {
        Parallel.ForEach(graph.Nodes, node => FindPathSingle(node, 0, null, new List<Node>()));
        return _maxPathSingle;
    }

    private static void FindPathSingle(Node currentNode, int currentPath, Node previous, List<Node> used)
    {
        if (currentPath > _maxPathSingle)
        { 
            _maxPath = currentPath;
        }
        used.Add(currentNode);
        var possiblePaths = currentNode.IncidentNodes.Where(x => x != previous && !used.Contains(x));

        foreach (var node in possiblePaths)
        {
            FindPathSingle(node, currentPath+1, currentNode, used);
        }
    }
}