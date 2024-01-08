using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace CWGraphs.Client.ViewModels;

public class NodeDto
{
    public int Number { get; set; }
    public int Size { get; set; } 
    public Point CenterPoint { get; set; }
    
    public NodeViewModel ConvertVoVM(Canvas canvas)
    {
        var vm = new NodeViewModel(canvas, Number, Size, CenterPoint);
   
        return vm;
    }
}

public class EdgeDTO
{
    public NodeDto Node1 { get; set; }
    public NodeDto Node2 { get; set; }

    public EdgeViewModel ConvertVoVM(Canvas canvas, List<NodeViewModel> nodes)
    {
        var node1 = nodes.First(x => Math.Abs(x.CenterPoint.X - Node1.CenterPoint.X) < 0.00001 && Math.Abs(x.CenterPoint.Y - Node1.CenterPoint.Y) < 0.00001);
        var node2 = nodes.First(x => Math.Abs(x.CenterPoint.X - Node2.CenterPoint.X) < 0.00001 && Math.Abs(x.CenterPoint.Y - Node2.CenterPoint.Y) < 0.00001);
        return new EdgeViewModel(canvas, node1, node2);
    }
}

public class GraphDTO
{
    public List<EdgeDTO>? Edges { get; set; }
    public List<NodeDto>? Nodes { get; set; }

    public GraphViewModel ConvertToVM(Canvas canvas)
    {
        var nodes = Nodes!.Select(node => node.ConvertVoVM(canvas)).ToList();
        var edges = Edges!.Select(edge => edge.ConvertVoVM(canvas, nodes)).ToList();
            
        return new GraphViewModel(edges, nodes);
    }
}