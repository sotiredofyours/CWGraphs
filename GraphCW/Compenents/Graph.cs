﻿namespace GraphCW;

public class Graph
{
    private Node[] nodes;

    public Graph(int nodesCount)
    {
        nodes = Enumerable.Range(0, nodesCount).Select(z => new Node(z)).ToArray();
    }

    public Node this[int index] => nodes[index];

    public IEnumerable<Node> Nodes
    {
        get
        {
            foreach (var node in nodes) yield return node;
        }
    }

    public void Connect(int index1, int index2)
    {
        Node.Connect(nodes[index1], nodes[index2], this);
    }

    public void Delete(Edge edge)
    {
        Node.Disconnect(edge);
    }

    public IEnumerable<Edge> Edges
    {
        get
        {
            return nodes.SelectMany(z => z.IncidentEdges).Distinct();
        }
    }

    public bool IsConnected(Node node1, Node node2)
    {
        return node1.IncidentNodes.Contains(node2);
    }
    
}