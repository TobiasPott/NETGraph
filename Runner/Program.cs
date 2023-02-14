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

public class MyGraph : GenericGraph<Node, UnweightedEdge>
{
    public MyGraph(IEnumerable<Node> vertices) : base(vertices)
    { }
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
                            new Node(VEnd)
                        };
        MyGraph g = new MyGraph(null);
        int iStart = g.addVertex(new Node(VStart));
        int iMid = g.addVertex(new Node(VMid));
        int iEnd = g.addVertex(new Node(VEnd));

        g.addEdge(new UnweightedEdge(iStart, iMid, true));
        g.addEdge(new UnweightedEdge(iMid, iEnd, true));

        Console.WriteLine(g.ToString());


        UnweightedGraph<Node> u = new UnweightedGraph<Node>(vertices, true, true);
        Console.WriteLine(u.ToString());

    }

}