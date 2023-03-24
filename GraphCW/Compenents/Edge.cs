namespace GraphCW;

/// <summary>
/// Represent edge of the graph.
/// </summary>
public class Edge
{
    public readonly Node First;
    public readonly Node Second;
    public Edge(Node first, Node second)
    {
        this.First = first;
        this.Second = second;
    }
    public bool IsIncident(Node node)
    {
        return First == node || Second == node;
    }
    public Node OtherNode(Node node)
    {
        if (!IsIncident(node)) throw new ArgumentException();
        if (First == node) return Second;
        return First;
    }
}