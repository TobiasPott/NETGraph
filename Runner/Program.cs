using System;
using System.Collections.Generic;
using Core;

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
        UnweightedGraph<Node> g = new UnweightedGraph<Node>();
        int iStart = g.addVertex(new Node(VStart));
        int iMid = g.addVertex(new Node(VMid));
        int iEnd = g.addVertex(new Node(VEnd));

        g.addEdge(new UnweightedEdge(iStart, iMid, true));
        g.addEdge(new UnweightedEdge(iMid, iEnd, true));
        g.addEdge(new UnweightedEdge(iMid, iEnd, true));
        g.addEdge(new UnweightedEdge(iMid, iEnd, true));
        Console.WriteLine($"{g} {g.edgeCount}");


        DistinctGraph<Node, UnweightedEdge> u = new DistinctGraph<Node, UnweightedEdge>(vertices);
        u.addEdge(new UnweightedEdge(iStart, iMid, true));
        u.addEdge(new UnweightedEdge(iMid, iEnd, true));
        u.addEdge(new UnweightedEdge(iMid, iEnd, true));
        u.addEdge(new UnweightedEdge(iMid, iEnd, true));
        Console.WriteLine($"{u} {u.edgeCount}");
    }

}