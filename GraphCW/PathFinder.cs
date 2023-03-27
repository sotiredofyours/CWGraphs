namespace GraphCW;

public static class PathFinder
{
    private static volatile int  _maxPath = 0;
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
        Parallel.ForEach(possiblePaths, node => FindPath(node, currentPath + 1, node, used));
    }
}