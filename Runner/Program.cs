﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NETGraph;
using NETGraph.Core;
using NETGraph.Core.BuiltIn;
using NETGraph.Core.Meta;
using NETGraph.Graphs;


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

    public static object GuidStringNode { get; private set; }

    public static void Main(string[] args)
    {
        MetaTypeRegistry.RegisterBuiltIn();

        #region Old Code
        //Node[] vertices = { new Node(VStart),
        //                    new Node(VMid),
        //                    new Node(VEnd),
        //                    new Node(VEnd)
        //                };

        //UnweightedGraph<Node> g = new UnweightedGraph<Node>();
        //int iStart = g.addVertex(new Node(VStart));
        //int iMid = g.addVertex(new Node(VMid));
        //int iEnd = g.addVertex(new Node(VEnd));


        //g.addEdge(new Edge(iStart, iMid, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //g.addEdge(new Edge(iMid, iEnd, true));
        //Console.WriteLine($"{g}{g.edgeCount}");

        /*
        DistinctGraph<Node, Edge<string>> u = new DistinctGraph<Node, Edge<string>>(vertices);
        u.addEdge(new Edge<string>(0, 1, NOutput, NInput));
        //u.addEdge(new NamedEdge<string>(0, 1, NInput, NOutput));
        u.addEdge(new Edge<string>(1, 2, NOutput, NInput));
        u.addEdge(new Edge<string>(2, 0, NOutput, NInput));
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
        DistinctGraph<Node, Edge<string>> rU = u.reversed();
        Console.WriteLine($"DFS: {rU.dfs(searchTarget, (x) => x == searchStart, x => x, _ => true)}");
        Console.WriteLine($"BFS: {rU.bfs(searchTarget, (x) => x == searchStart, x => x, _ => true)}");


        List<List<Node>> cycles = u.detectCycles();
        Console.WriteLine($"Detected Cycles: {cycles.Count} {string.Join(", ", cycles.Select(x => x.Count))}");

        List<List<Edge<string>>> cyclesEdges = u.detectCyclesAsEdges();
        Console.WriteLine($"Detected Cycles: {cyclesEdges.Count} {string.Join(", ", cyclesEdges.Select(x => x.Count))}");
        */

        /*
        FloatData floatScalar = new FloatData(1.0f);
        FloatData floatArray = new FloatData(Enumerable.Range(0, 10).Select(i => (float)i), false);
        FloatData floatArray2 = new FloatData(Enumerable.Range(0, 10).Select(i => (float)i), false);
        FloatData floatDict = new FloatData(new KeyValuePair<string, float>[] {
            new KeyValuePair<string, float>("x", 0.0f),
            new KeyValuePair<string, float>("y", 0.0f),
            new KeyValuePair<string, float>("z", 0.0f)
        }, false);

        FloatData[] data = new FloatData[] { floatScalar, floatArray, floatDict };
        Console.WriteLine($"{string.Join<FloatData>(Environment.NewLine, data)}");
        Console.WriteLine();

        Console.WriteLine($"{floatArray} == {floatArray2} : {floatArray.matchWithValue(floatArray2)}");
        floatArray.SetAt(3, -22.0f);
        floatDict.SetAt("x", 3.335f);
        Console.WriteLine($"{string.Join<FloatData>(Environment.NewLine, data)}");
        Console.WriteLine();

        Console.WriteLine($"{floatArray} == {floatArray2} : {floatArray.matchWithValue(floatArray2)}");
        */
        #endregion

        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3Int vec3IntData = new Vector3Int(1, 10, 0);
        LibMath libMath = LibMath.Instance;


        // binding to actual data objeect => reesults in resolvable DataQuery
        MethodResolver addQuery = new MethodResolver(libMath, "int32 add(int32, int32, int32)", vec3IntData.z, vec3IntData.x, vec3IntData.y);
        TimeStamp(sw, vec3IntData.ToString());

        addQuery.resolve(); // execute method
        TimeStamp(sw, vec3IntData.ToString());

        vec3IntData.assign("x", 5);
        addQuery = new MethodResolver(libMath, "int32 add(int32, int32, int32)", vec3IntData.z, vec3IntData.x, vec3IntData.z);
        for (int i = 0; i < 10; i++)
            addQuery.resolve(); // execute method
        TimeStamp(sw, vec3IntData.ToString());

        vec3IntData.assign("x", 12);
        for (int i = 0; i < 5; i++)
            addQuery.resolve(); // execute method

        TimeStamp(sw, vec3IntData.ToString());

        Console.WriteLine();
    }

    public static void TimeStamp(Stopwatch sw, string additional)
    {
        Console.WriteLine(sw.Elapsed.TotalMicroseconds.ToString("0000000") + ": " + additional);

    }

}