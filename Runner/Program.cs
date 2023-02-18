using System;
using System.Collections.Generic;
using NETGraph;

public class Node : IEquatable<Node>
{
    string name = string.Empty;
    List<object> data = new List<object>();

    public Node(string name)
    {
        this.name = name;
    }

    bool IEquatable<Node>.Equals(Node other)
    {
        if (other != null)
            return other.name.Equals(this.name);
        return false;
    }

    public override string ToString()
    {
        return $"{name} ({data.Count})";
    }
}


public class Program
{
    public const string NInput = "input";
    public const string NOutput = "output";
    public const string VStart = "Start";
    public const string VMid = "Mitte";
    public const string VEnd = "Ende";

    public static void Main(string[] args)
    {
        Node[] vertices = { new Node(VStart),
                            new Node(VMid),
                            new Node(VEnd),
                            new Node(VEnd)
                        };
        //UnweightedGraph<Node> g = new UnweightedGraph<Node>();
        //int iStart = g.addVertex(new Node(VStart));
        //int iMid = g.addVertex(new Node(VMid));
        //int iEnd = g.addVertex(new Node(VEnd));


        //g.addEdge(new Edge(iStart, iMid, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //Console.WriteLine($"{g}{g.edgeCount}");


        DistinctGraph<Node, NamedEdge<string>> u = new DistinctGraph<Node, NamedEdge<string>>(vertices);
        u.addEdge(new NamedEdge<string>(0, 1, NOutput, NInput));
        u.addEdge(new NamedEdge<string>(0, 1, NInput, NOutput));
        u.addEdge(new NamedEdge<string>(1, 2, NOutput, NInput));
        Console.WriteLine($"{u}{u.edgeCount}");

        Console.WriteLine($"{string.Join(Environment.NewLine, u.edgesForIndex(0))}");

        Console.WriteLine();
        Console.WriteLine("Search:");
        int searchStart = 0;
        int searchTarget = 2;
        Node targetNode = u.vertexAtIndex(1);
        Console.WriteLine($"DFS: {u.dfs(searchStart, (x) => x == searchTarget, x => x, _ => true)}");
        Console.WriteLine($"BFS: {u.bfs(searchStart, (x) => x == searchTarget, x => x, _ => true)}");


        Console.WriteLine();
        Console.WriteLine("Search Reversed:");
        DistinctGraph<Node, NamedEdge<string>> rU = u.reversed<DistinctGraph<Node, NamedEdge<string>>, Node, NamedEdge<string>>();
        Console.WriteLine($"DFS: {rU.dfs(searchTarget, (x) => x == searchStart, x => x, _ => true)}");
        Console.WriteLine($"BFS: {rU.bfs(searchTarget, (x) => x == searchStart, x => x, _ => true)}");
    }

}