using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using CWGraphs.Client.ViewModels;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Newtonsoft.Json;
using Path = System.IO.Path;


namespace CWGraphs.Client.Views;

public partial class MainWindow : Window
{
    private readonly Canvas _canvas;
    private int _counter;
    private  List<NodeViewModel>? _nodes = new();
    private  List<EdgeViewModel>? _edges = new();
    
    private Line? _connectingLine;
    private bool isDisconnecting;
    private bool isRemovingNode;
    private bool isConnecting;
    private NodeViewModel? firstNode;
    private ProgressBar? _progressBar;
    private Slider? _sizeOfNode;

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        _progressBar = this.Find<ProgressBar>("ProgressBar");
        _canvas = this.Find<Canvas>("MainCanvas")!;
        _sizeOfNode = this.Find<Slider>("Slider");
        _canvas.PointerPressed += CanvasOnPointerPressed;
    }
    public void AddNode(object sender, RoutedEventArgs args)
    {
        _nodes.Add(new NodeViewModel(_canvas, ++_counter, (int) _sizeOfNode.Value));
    }

    public void ConnectNodes(object sender, RoutedEventArgs args)
    {
        isConnecting = !isConnecting;
        Cursor = isConnecting ? new Cursor(StandardCursorType.Hand) : new Cursor(default);
    }

    public void RemoveNode(object sender, RoutedEventArgs args)
    {
        isRemovingNode = !isRemovingNode;
        Cursor = isRemovingNode ? new Cursor(StandardCursorType.Hand) : new Cursor(default);
    }

    public async void CalculatePath(object sender, RoutedEventArgs args)
    {
        _progressBar.IsVisible = true;
        var task =  await new GraphViewModel(_edges, _nodes).CalculatePath();
        if (task is null)
        {
            _progressBar.IsVisible = false;
            return;
        }

        for (var i = 0; i < task.Count; i++)
        {
            var node = _nodes.FirstOrDefault(x => x.Number == task[i]);
            node?.Highlight();
            await Task.Delay(1000);
            if (i + 1 == task.Count) continue;
            var nextNode = _nodes.FirstOrDefault(x => x.Number == task[i + 1]);
            var edge = _edges.FirstOrDefault(x =>
                (x.Node1 == node && x.Node2 == nextNode) || (x.Node1 == nextNode && x.Node2 == node));
            edge?.Highlight();
            await Task.Delay(1000);
        }
        _progressBar.IsVisible = false;

        await Task.Delay(5000);
        foreach (var edge in _edges)
        {
            edge.Unhighlight();
        }
        foreach (var node in _nodes)
        {
            node.Unhighlight();
        }
    }

    public void ShowAbout(object sender, RoutedEventArgs args)
    {
        var window = this;
        var about = new About();
        about.ShowDialog(window);
    }

    public void Export(object sender, RoutedEventArgs args)
    {
        var graph = new GraphViewModel(_edges, _nodes);
        var json = JsonConvert.SerializeObject(graph);
        SaveToFile(json);
    }

    public async void Import(object sender, RoutedEventArgs args)
    {
        var path = await ChooseJsonFile();
        if (path is null) return;
        var graph = JsonConvert.DeserializeObject<GraphDTO>(File.ReadAllText(path.AbsolutePath));
        var graphVM = graph?.ConvertToVM(_canvas);
        ClearCanvas();
        _edges = graphVM.Edges;
        _nodes = graphVM.Nodes;
        _counter += _nodes.Count;
    }

    private void ClearCanvas()
    {
        foreach (var edge in _edges)
        {
            edge.Remove();
        }
        
        foreach (var node in _nodes)
        {
            node.Remove();
        }
    }
    private async Task<Uri> ChooseJsonFile()
    {
        var openFileDialog = await new Window().StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
        });
        
        return openFileDialog.FirstOrDefault()?.Path;
    }

    private void DisconnectNodes(object sender, RoutedEventArgs args)
    {
        isDisconnecting = !isDisconnecting;
        Cursor = isDisconnecting ? new Cursor(StandardCursorType.Hand) : new Cursor(default);
    }

    private void CanvasOnPointerPressed(object? sender, PointerEventArgs e)
    {
        var cursorPosition = e.GetPosition(_canvas);
        if (isConnecting)
            OnConnect(cursorPosition);
        if (isDisconnecting)
            OnDisconnect(cursorPosition);
        if (isRemovingNode)
            OnRemovingNode(cursorPosition);
    }

    private void OnRemovingNode(Point cursorPosition)
    {
        var selectedNode = _nodes.FirstOrDefault(x => x.IsMouseOverEllipse(cursorPosition));
        if (selectedNode == null) return;
        selectedNode.Remove();
        var connectedEdges = _edges
            .Where(x => x.Node1 == selectedNode || x.Node2 == selectedNode)
            .ToList();
        foreach (var edge in connectedEdges)
        {
            edge.Remove();
            _edges.Remove(edge);
        }

        _nodes.Remove(selectedNode);
        isRemovingNode = false;
        Cursor = new Cursor(default);
    }
    
    private async void SaveToFile(string content)
    {
        var dialog = new OpenFolderDialog();
        var folderPath = await dialog.ShowAsync(this);

        if (string.IsNullOrEmpty(folderPath)) return;
        var filePath = Path.Combine(folderPath, "graph.json");
        File.WriteAllText(filePath, content);
    }
    private void OnDisconnect(Point cursorPosition)
    {
         var selectedEdge = _edges.FirstOrDefault(x => x.IsMouseOverLine(cursorPosition));
         if (selectedEdge == null) return;
         selectedEdge.Remove();
         _edges.Remove(selectedEdge);
         isDisconnecting = false;
         Cursor = new Cursor(default);
    }

    private void OnConnect(Point cursorPosition)
    {
        var selectedNode = _nodes.FirstOrDefault(x => x.IsMouseOverEllipse(cursorPosition));
        if (firstNode != null)
        {
            if (selectedNode != null) 
                _edges.Add(new EdgeViewModel(_canvas, firstNode, selectedNode));
            isConnecting = false;
            Cursor = new Cursor(default);
            firstNode = null;
            return;
        }

        firstNode = selectedNode;
    }
}