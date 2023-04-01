namespace GraphCW;

public static class PathFinder
{
    private static volatile int  _maxPath;
    private static volatile int  _maxPathSingle;
    private static SinglyLinkedList<Node> _path;
    private static SinglyLinkedList<Node> _pathSingle;
    private static readonly object _obj = new();
    
    public static SinglyLinkedList<Node> FindLongestPath(Graph graph)
    {
        Parallel.ForEach(graph.Nodes, node => FindPath(node, null, new List<Node>()));
        
        return _path; // returns null if graph.Nodes < 2.
    }

    private static void FindPath(Node currentNode, SinglyLinkedList<Node> path, ICollection<Node> used)
    {
        used.Add(currentNode);
        var possiblePaths = currentNode.IncidentNodes
            .Where(x =>
            {
                if (path == null) return true;
                return x != path.Value && !used.Contains(x);
            })
            .ToList();

        if (!possiblePaths.Any() && path != null)
        {
            lock (_obj)
            {
               if (path.Length > _maxPath)
               { 
                   _maxPath = path.Length;
                   _path = path;
               } 
            }
        }
        
        foreach (var node in possiblePaths)
        {
            FindPath(node,
                path != null
                    ? new SinglyLinkedList<Node>(node, path)
                    : new SinglyLinkedList<Node>(node, new SinglyLinkedList<Node>(currentNode)), 
                used);
        }
    }

    public static SinglyLinkedList<Node> FindLongestPathSingle(Graph graph)
    {
        foreach (var node in graph.Nodes)
        {
            FindPathSingle(node, null, new List<Node>());
        }
         
        return _pathSingle;
    }
    
    private static void FindPathSingle(Node currentNode, SinglyLinkedList<Node> path, ICollection<Node> used)
    {
        used.Add(currentNode);
        var possiblePaths = currentNode.IncidentNodes
            .Where(x =>
            {
                if (path == null) return true;
                return x != path.Value && !used.Contains(x);
            })
            .ToList();

        if (!possiblePaths.Any() && path != null)
        {
            if (path.Length > _maxPathSingle)
            { 
                _maxPathSingle = path.Length;
                _pathSingle = path;
            }
        }
        
        foreach (var node in possiblePaths)
        {
            FindPath(node,
                path != null
                    ? new SinglyLinkedList<Node>(node, path)
                    : new SinglyLinkedList<Node>(node, new SinglyLinkedList<Node>(currentNode)), 
                used);
        }
    }
}