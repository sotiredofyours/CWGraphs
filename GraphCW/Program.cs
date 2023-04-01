using GraphCW;

Console.WriteLine("New graph");
Console.WriteLine("Enter the number of nodes:");
var nodes = int.Parse(Console.ReadLine()!);

Console.WriteLine("Enter the number of edges:");
var edges = int.Parse(Console.ReadLine()!);

Console.WriteLine("Enter edges in format: <from node> - <to node> \n NOTE: nodes numbers in range [0..nodes.Length - 1]");
var connections = new string[edges];

for (int i = 0; i < edges; i++)
{
  connections[i] = Console.ReadLine()!;
}

var graph = GraphCreator.CreateGraph(nodes, edges, connections);
var path = PathFinder.FindLongestPath(graph);

Console.WriteLine($"Longest simple path = { path.Length }");
Console.WriteLine(string.Join(" -> ", path));


