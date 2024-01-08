using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace CWGraphs.Client.ViewModels;

public class EdgeViewModel
{
    private readonly Canvas _canvas;
    public NodeViewModel Node1 { get; set; }
    public NodeViewModel Node2 { get; set; }
    private Line _line;
    
    public EdgeViewModel(Canvas canvas, NodeViewModel node1, NodeViewModel node2)
    {
        _canvas = canvas;
        var line = new Line
        {
            StartPoint = node1.CenterPoint,
            EndPoint = node2.CenterPoint,
            StrokeThickness = 5,
            Stroke = Brushes.White,
            ZIndex = -100 
        };
        _line = line;
        Node1 = node1;
        Node2 = node2;
        
        Node1.PropertyChanged += OnNodeMoved;
        Node2.PropertyChanged += OnNodeMoved;

        _line.PointerEntered += OnPointerEntered;
        _line.PointerExited += OnPointerExited;
        
        _canvas.Children.Add(line);
    }

    private void OnNodeMoved(object? sender, PropertyChangedEventArgs args)
    {
        _line.StartPoint = Node1.CenterPoint;
        _line.EndPoint = Node2.CenterPoint;
    }
    
    private void OnPointerEntered(object? o, PointerEventArgs e)
    {
        if (o is Line line)
        {
            line.Stroke = Brushes.Red; 
        }
    }
    
    private void OnPointerExited(object? o, PointerEventArgs e)
    {
        if (o is Line line)
        {
            line.Stroke = Brushes.White;
        }
    }

    public bool IsMouseOverLine(Point mousePosition)
    {
        
        var k = (float)(_line.EndPoint.Y - _line.StartPoint.Y) / (_line.EndPoint.X - _line.StartPoint.X);
        var b = _line.StartPoint.Y - k * _line.StartPoint.X;

        var yOnLine = k * mousePosition.X + b;

        return Math.Abs(mousePosition.Y - yOnLine) <= 10;
    }

    public void Remove()
    {
        _canvas.Children.Remove(_line);
    }

    public void Highlight()
    {
        _line.Stroke = Brushes.Coral;
    }

    public void Unhighlight()
    {
        _line.Stroke = Brushes.White;
    }
}