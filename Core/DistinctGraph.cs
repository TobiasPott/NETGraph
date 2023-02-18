﻿using System;
using System.Collections.Generic;

namespace NETGraph
{

    /// An implementation Graph that ensures there are no pairs of equal vertices and no repeated edges.
    public class DistinctGraph<V, E> : Graph<V, E> where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
    {

        public DistinctGraph() : base()
        { }
        public DistinctGraph(IEnumerable<V> vertices) : base()
        {
            foreach (V vertex in vertices)
                this.addVertex(vertex);
        }


        /// Add a vertex to the graph if no equal vertex already belongs to the Graph. O(n)
        public override int addVertex(V vertex)
        {
            int index = indexOfVertex(vertex);
            if (index == -1)
                return base.addVertex(vertex);
            return index;
        }
        /// Add an edge to the graph. Only allow the edge to be added once
        public override void addEdge(E edge)
        {
            if (!edgeExists(edge))
                edges[edge.u].Add(edge);
            if (!edge.directed)
            {
                E reversedEdge = edge.reversed;
                if (!edgeExists(reversedEdge))
                    edges[edge.v].Add(reversedEdge);
            }
        }
    }
}

/*


/// An implementation Graph that ensures there are no pairs of equal vertices and no repeated edges.
open class UniqueElementsGraph<V: Equatable & Codable, E: Edge & Equatable>: Graph {

    /// Init the Graph with vertices, but removes duplicates. O(n^2)
    required public init(vertices: [V]) {
        for vertex in vertices {
            _ = self.addVertex(vertex) // make sure to call our version
        }
    }
}

extension UniqueElementsGraph where E == UnweightedEdge {

    private func addEdgesForPath(withIndices indices: [Int], directed: Bool) {
        for i in 0..<indices.count - 1 {
            addEdge(fromIndex: indices[i], toIndex: indices[i+1], directed: directed)
        }
    }

    /// Initialize an UniqueElementsGraph consisting of path.
    ///
    /// The resulting graph has the vertices in path and an edge between
    /// each pair of consecutive vertices in path.
    ///
    /// If path is an empty array, the resulting graph is the empty graph.
    /// If path is an array with a single vertex, the resulting graph has that vertex and no edges.
    ///
    /// - Parameters:
    ///   - path: An array of vertices representing a path.
    ///   - directed: If false, undirected edges are created.
    ///               If true, edges are directed from vertex i to vertex i+1 in path.
    ///               Default is false.
    public static func withPath(_ path: [V], directed: Bool = false) -> UniqueElementsGraph {
        let g = UniqueElementsGraph(vertices: path)

        guard path.count >= 2 else {
            return g
        }

        let indices = path.map({ g.indexOfVertex($0)! })
        g.addEdgesForPath(withIndices: indices, directed: directed)
        return g
    }

    /// Initialize an UniqueElementsGraph consisting of cycle.
    ///
    /// The resulting graph has the vertices in cycle and an edge between
    /// each pair of consecutive vertices in cycle,
    /// plus an edge between the last and the first vertices.
    ///
    /// If path is an empty array, the resulting graph is the empty graph.
    /// If path is an array with a single vertex, the resulting graph has the vertex
    /// and a single edge to itself if directed is true.
    /// If directed is false the resulting graph has the vertex and two edges to itself.
    ///
    /// - Parameters:
    ///   - cycle: An array of vertices representing a cycle.
    ///   - directed: If false, undirected edges are created.
    ///               If true, edges are directed from vertex i to vertex i+1 in cycle.
    ///               Default is false.
    public static func withCycle(_ cycle: [V], directed: Bool = false) -> UniqueElementsGraph {
        let g = UniqueElementsGraph(vertices: cycle)

        guard cycle.count >= 2 else {
            if let v = cycle.first {
                let index = g.addVertex(v)
                g.addEdge(fromIndex: index, toIndex: index)
            }
            return g
        }

        let indices = cycle.map({ g.indexOfVertex($0)! })
        g.addEdgesForPath(withIndices: indices, directed: directed)
        g.addEdge(fromIndex: indices.last!, toIndex: indices.first!, directed: directed)
        return g
    }

    private struct QueueElement<V> {
        let v: V
        let previousIndex: Int
    }

    /// Construct a UniqueElementsGraph by repeatedly applying a recursion function to a vertex and adding them to the graph.
    ///
    /// The recursion function is only called on a vertex when visited for the first time.
    ///
    /// - Parameter recursion: A function that returns the neighbouring vertices for a given visited vertex.
    /// - Parameter initialVertex: The first vertex to which the recursion function is applied.
    public static func fromRecursion(_ recursion: (V) -> [V], startingWith initialVertex: V) -> UniqueElementsGraph {
        return self.fromRecursion(recursion, selectingVertex: { $0 }, startingWith: initialVertex)
    }

    /// Construct a UniqueElementsGraph by repeatedly applying a recursion function to some elements and adding the corresponding vertex to the graph.
    ///
    /// The recursion function is only called on an element when visited for the first time.
    ///
    /// - Parameter recursion: A function that returns the neighbouring elements for a given visited element.
    /// - Parameter vertexFor: A function that returns the vertex that will be added to the graph for each visited element.
    /// - Parameter initialElement: The first element to which the recursion function is applied.
    public static func fromRecursion<T>(_ recursion: (T) -> [T], selectingVertex vertexFor: (T) -> V, startingWith initialElement: T) -> UniqueElementsGraph {
        let g = UniqueElementsGraph(vertices: [])

        let queue = Queue<QueueElement<T>>()

        g.vertices.append(vertexFor(initialElement))
        g.edges.append([E]())
        recursion(initialElement).forEach { v in
            queue.push(QueueElement(v: v, previousIndex: 0))
        }

        while !queue.isEmpty {
            let element = queue.pop()
            let (e, previousIndex) = (element.v, element.previousIndex)
            let u = vertexFor(e)
            let uIndex = g.indexOfVertex(u) ?? {
                g.vertices.append(u)
                g.edges.append([E]())

                let uIndex = g.vertices.count - 1

                recursion(e).forEach { v in
                    queue.push(QueueElement(v: v, previousIndex: uIndex))
                }
                return uIndex
                }()

            g.addEdge(fromIndex: previousIndex, toIndex: uIndex, directed: true)
        }

        return g
    }
}

extension UniqueElementsGraph where V: Hashable, E == UnweightedEdge {
    public static func withPath(_ path: [V], directed: Bool = false) -> UniqueElementsGraph {
        let g = UniqueElementsGraph()

        guard path.count >= 2 else {
            if let v = path.first {
                _ = g.addVertex(v)
            }
            return g
        }

        let indices = g.indicesForPath(path)
        g.addEdgesForPath(withIndices: indices, directed: directed)
        return g
    }


    public static func withCycle(_ cycle: [V], directed: Bool = false) -> UniqueElementsGraph {
        let g = UniqueElementsGraph()

        guard cycle.count >= 2 else {
            if let v = cycle.first {
                let index = g.addVertex(v)
                g.addEdge(fromIndex: index, toIndex: index)
            }
            return g
        }

        let indices = g.indicesForPath(cycle)
        g.addEdgesForPath(withIndices: indices, directed: directed)
        g.addEdge(fromIndex: indices.last!, toIndex: indices.first!, directed: directed)
        return g
    }

    private func indicesForPath(_ path: [V]) -> [Int] {
        var indices: [Int] = []
        var indexForVertex: Dictionary<V, Int> = [:]

        for v in path {
            if let index = indexForVertex[v] {
                indices.append(index)
            } else {
                let index = addVertex(v)
                indices.append(index)
                indexForVertex[v] = index
            }
        }
        return indices
    }
}


*/