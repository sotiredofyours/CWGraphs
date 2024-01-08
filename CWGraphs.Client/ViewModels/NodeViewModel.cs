using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;

namespace CWGraphs.Client.ViewModels;

public class NodeViewModel : INotifyPropertyChanged
{
    private Point _centerPoint;
    public int Size { get; set; }
    public Point CenterPoint
    {
        get => _centerPoint;
        set
        {
            _centerPoint = value;
            OnPropertyChanged();  
        }
    }
    private readonly TextBlock _number;
    private readonly Ellipse _ellipse;
    private readonly Canvas _canvas;

    public int Number => Convert.ToInt32(_number.Text);
    
    public NodeViewModel(Canvas canvas, int number, int size, Point centerPoint = default)
    {
        Size = size;
        _centerPoint = centerPoint;
        _canvas = canvas;
        _ellipse = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = Brushes.Gainsboro,
            Stroke = Brushes.White,
            StrokeThickness = 5d,
        };
        
        _number = new TextBlock
        {
            Text = number.ToString(), 
            Foreground = Brushes.Black,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            FontSize = size / 2
        };
        
        _canvas.Children.Add(_ellipse);
        Canvas.SetLeft(_ellipse, _centerPoint.X == 0 ? 100 : _centerPoint.X - _ellipse.Width / 2);
        Canvas.SetTop(_ellipse, _centerPoint.Y == 0 ? 100 : _centerPoint.Y - _ellipse.Height / 2);
        Canvas.SetLeft(_number, Canvas.GetLeft(_ellipse) + (_ellipse.Width - _number.Bounds.Width) / 3);
        Canvas.SetTop(_number, Canvas.GetTop(_ellipse) + (_ellipse.Height - _number.Bounds.Height) / 3);
        _canvas.Children.Add(_number);
        _ellipse.PointerMoved += OnPointerMoved;
        _ellipse.PointerPressed += OnPressed;
        _ellipse.PointerExited += OnPointerExited;
        _ellipse.PointerEntered += OnPointerEntered;
    }

    private void OnPointerMoved(object? o, PointerEventArgs e)
    {
        if (!e.GetCurrentPoint(_canvas).Properties.IsLeftButtonPressed) return;
        var currentPosition = e.GetPosition(_canvas);
        var offset = currentPosition - CenterPoint;
        Canvas.SetLeft(_ellipse, Canvas.GetLeft(_ellipse) + offset.X);
        Canvas.SetTop(_ellipse, Canvas.GetTop(_ellipse) + offset.Y);
        Canvas.SetLeft(_number, Canvas.GetLeft(_ellipse) + (_ellipse.Width - _number.Bounds.Width) / 2);
        Canvas.SetTop(_number, Canvas.GetTop(_ellipse) + (_ellipse.Height - _number.Bounds.Height) / 2);
        CenterPoint = currentPosition;
    }

    private void OnPressed(object? o, PointerEventArgs e)
    {
        CenterPoint = e.GetPosition(_canvas);
    }

    private void OnPointerEntered(object? o, PointerEventArgs e)
    {
        if (o is Ellipse ell)
        {
            ell.Stroke = Brushes.Red; 
        }
    }
    
    private void OnPointerExited(object? o, PointerEventArgs e)
    {
        if (o is Ellipse ell)
        {
            ell.Stroke = Brushes.White;
        }
    }
    
    public bool IsMouseOverEllipse(Point mousePosition)
    {
        var ellipseRadiusX = _ellipse.Width / 2;
        var ellipseRadiusY = _ellipse.Height / 2;
        
        var distance = Math.Sqrt(Math.Pow(mousePosition.X - CenterPoint.X, 2) + Math.Pow(mousePosition.Y - CenterPoint.Y, 2));

        return distance <= ellipseRadiusX && distance <= ellipseRadiusY;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    public void Remove()
    {
        _canvas.Children.Remove(_ellipse);
        _canvas.Children.Remove(_number);
    }

    public void Highlight()
    {
        _ellipse.Fill = Brushes.Coral;
    }

    public void Unhighlight()
    {
        _ellipse.Fill = Brushes.White;
    }
}