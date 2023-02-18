﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{

    // ToDo: Continue working on left out utilities

    // Stub class for missing helper and utility methods for different graph types and situations
    // The following list refer to the SwiftGraph code file names and their respective content
    //      (marked entries are up for reconsideration)
    //  *   [+] Cycle
    //  *   [0] MST 
    //  *   [+] Reversed
    //  *   [ ] Search
    //  *   [ ] Sort
    //  *   [ ] Union
    //  *   
    //  *

    public static class Search
    {
        public enum Algorithm
        {
            DepthFirst,
            BreadthFirst
        }

        public static int search<V, E>(this Graph<V, E> graph, int fromIndex, Func<int, bool> goalTest, Func<IEnumerable<E>, IEnumerable<E>> visitOrder, Func<E, bool> reducer, Algorithm algorithm)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    return dfs(graph, fromIndex, goalTest, visitOrder, reducer);
                case Algorithm.BreadthFirst:
                    return bfs(graph, fromIndex, goalTest, visitOrder, reducer);
            }
            return -1;
        }
        public static IEnumerable<E> search<V, E>(this Graph<V, E> graph, int fromIndex, Func<V, bool> goalTest, Algorithm algorithm)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {

            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    return dfs(graph, fromIndex, goalTest);
                case Algorithm.BreadthFirst:
                    return bfs(graph, fromIndex, goalTest);
            }
            return Enumerable.Empty<E>();
        }
        public static IEnumerable<E> search<V, E>(this Graph<V, E> graph, V from, Func<V, bool> goalTest, Algorithm algorithm)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    return dfs(graph, from, goalTest);
                case Algorithm.BreadthFirst:
                    return bfs(graph, from, goalTest);
            }
            return Enumerable.Empty<E>();
        }
        public static IEnumerable<E> search<V, E>(this Graph<V, E> graph, int fromIndex, int toIndex, Algorithm algorithm)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    return dfs(graph, fromIndex, toIndex);
                case Algorithm.BreadthFirst:
                    return bfs(graph, fromIndex, toIndex);
            }
            return Enumerable.Empty<E>();
        }
        public static IEnumerable<E> search<V, E>(this Graph<V, E> graph, V from, V to, Algorithm algorithm)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            switch (algorithm)
            {
                case Algorithm.DepthFirst:
                    return dfs(graph, from, to);
                case Algorithm.BreadthFirst:
                    return bfs(graph, from, to);
            }
            return Enumerable.Empty<E>();
        }


        #region Depth-First Algorithm
        /// Perform a computation over the graph visiting the vertices using a
        /// depth-first algorithm.
        /// - Parameters:
        ///   - initalVertexIndex: The index of the vertex that will be visited first.
        ///   - goalTest: Returns true if a given vertex index is a goal.
        ///   - visitOrder: A closure that orders an array of edges. For each visited vertex, the array
        ///                 of its outgoing edges will be passed to this closure and the neighbours will
        ///                 be visited in the order of the resulting array.
        ///   - reducer: A closure that is fed with each visited edge. The input parameter
        ///              is the edge from the previously visited vertex to the currently visited vertex.
        ///              If the return value is false, the neighbours of the currently visited vertex won't be visited.
        /// - Returns: The index of the first vertex found to satisfy goalTest or nil if no vertex is found.
        public static int dfs<V, E>(this Graph<V, E> graph, int fromIndex,
                            Func<int, bool> goalTest,
                            Func<IEnumerable<E>, IEnumerable<E>> visitOrder,
                            Func<E, bool> reducer)
        where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            if (goalTest(fromIndex))
                return fromIndex;

            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Stack<E> stack = new Stack<E>();

            visited[fromIndex] = true;
            IEnumerable<E> neighbours = graph.edgesForIndex(fromIndex);
            // iterate over all neighbour edges and push them onto stack
            foreach (E edge in (visitOrder?.Invoke(neighbours) ?? neighbours))
            {
                if (!visited[edge.v])
                    stack.Push(edge);
            }
            // process every neighbour edge until none left
            while (stack.Count > 0)
            {
                E edge = stack.Pop();
                int v = edge.v;
                if (visited[v])
                    continue;

                if (goalTest(v))
                    return v;

                bool shouldVisitNeighbours = reducer?.Invoke(edge) ?? true;
                if (shouldVisitNeighbours)
                {
                    visited[v] = true;
                    IEnumerable<E> edgeNeighbours = graph.edgesForIndex(v);
                    foreach (E e in (visitOrder?.Invoke(edgeNeighbours) ?? edgeNeighbours))
                        if (!visited[e.v])
                            stack.Push(e);
                }

            }
            return -1;
        }

        /// Find a route from a vertex to the first that satisfies goalTest()
        /// using a depth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> dfs<V, E>(this Graph<V, E> graph, int fromIndex, Func<V, bool> goalTest)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Stack<int> stack = new Stack<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            stack.Push(fromIndex);

            while (stack.Count > 0)
            {
                int v = stack.Pop();
                if (visited[v])
                    continue;

                visited[v] = true;
                // return path if index fullfills goalTest delegate
                if (goalTest(graph.vertexAtIndex(v)))
                    return pathDictToPath(fromIndex, v, pathDict);

                foreach (E edge in graph.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        stack.Push(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return Enumerable.Empty<E>();
        }

        /// Find a route from a vertex to the first that satisfies goalTest()
        /// using a depth-first search.
        /// - Parameters:
        /// - parameter from: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> dfs<V, E>(this Graph<V, E> graph, V from, Func<V, bool> goalTest)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            int u = graph.indexOfVertex(from);
            if (u != -1)
                return graph.dfs(u, goalTest);
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a depth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter toIndex: The index of the ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> dfs<V, E>(this Graph<V, E> graph, int fromIndex, int toIndex)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Stack<int> stack = new Stack<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            stack.Push(fromIndex);

            while (stack.Count > 0)
            {
                int v = stack.Pop();
                if (visited[v])
                    continue;

                visited[v] = true;
                // return path if index fullfills goalTest delegate
                if (v == toIndex)
                    return pathDictToPath(fromIndex, toIndex, pathDict);

                foreach (E edge in graph.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        stack.Push(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a depth-first search.
        /// - Parameters:
        /// - parameter from: The starting vertex.
        /// - parameter to: The ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> dfs<V, E>(this Graph<V, E> graph, V from, V to)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            int u = graph.indexOfVertex(from);
            if (u != -1)
            {
                int v = graph.indexOfVertex(to);
                if (v != -1)
                    return graph.dfs(u, v);
            }
            return Enumerable.Empty<E>();
        }

        #endregion

        #region Breadth-First Algorithm
        /// Perform a computation over the graph visiting the vertices using a breadth-first algorithm.
        /// - Parameters:
        ///   - initalVertexIndex: The index of the vertex that will be visited first.
        ///   - goalTest: Returns true if a given vertex index is a goal.
        ///   - visitOrder: A closure that orders an array of edges. For each visited vertex, the array
        ///                 of its outgoing edges will be passed to this closure and the neighbours will
        ///                 be visited in the order of the resulting array.
        ///   - reducer: A closure that is fed with each visited edge. The input parameter
        ///              is the edge from the previously visited vertex to the currently visited vertex.
        ///              If the return value is false, the neighbours of the currently visited vertex won't be visited.
        /// - Returns: The index of the first vertex found to satisfy goalTest or nil if no vertex is found.
        public static int bfs<V, E>(this Graph<V, E> graph, int fromIndex,
                        Func<int, bool> goalTest,
                        Func<IEnumerable<E>, IEnumerable<E>> visitOrder,
                        Func<E, bool> reducer)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            if (goalTest(fromIndex))
                return fromIndex;

            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Queue<E> queue = new Queue<E>();

            visited[fromIndex] = true;
            IEnumerable<E> neighbours = graph.edgesForIndex(fromIndex);
            // iterate over all neighbour edges and push them onto stack
            foreach (E edge in (visitOrder?.Invoke(neighbours) ?? neighbours))
            {
                if (!visited[edge.v])
                {
                    queue.Enqueue(edge);
                    visited[edge.v] = true;
                }
            }
            // process every neighbour edge until none left
            while (queue.Count > 0)
            {
                E edge = queue.Dequeue();
                int v = edge.v;

                if (goalTest(v))
                    return v;

                bool shouldVisitNeighbours = reducer?.Invoke(edge) ?? true;
                if (shouldVisitNeighbours)
                {
                    IEnumerable<E> edgeNeighbours = graph.edgesForIndex(v);
                    foreach (E e in (visitOrder?.Invoke(edgeNeighbours) ?? edgeNeighbours))
                    {
                        if (!visited[e.v])
                        {
                            queue.Enqueue(e);
                            visited[e.v] = true;
                        }
                    }
                }

            }
            return -1;
        }

        /// Find a route from a vertex to the first that satisfies goalTest() using a breadth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> bfs<V, E>(this Graph<V, E> graph, int fromIndex, Func<V, bool> goalTest)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Queue<int> queue = new Queue<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            queue.Enqueue(fromIndex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                if (goalTest.Invoke(graph.vertexAtIndex(v)))
                    return pathDictToPath(fromIndex, v, pathDict);


                visited[v] = true;
                // return path if index fullfills goalTest delegate
                if (goalTest(graph.vertexAtIndex(v)))
                    return pathDictToPath(fromIndex, v, pathDict);

                foreach (E edge in graph.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        visited[edge.v] = true;
                        queue.Enqueue(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return Enumerable.Empty<E>();
        }

        /// Find a route from a vertex to the first that satisfies goalTest() using a breadth-first search.
        /// - Parameters:
        /// - parameter from: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> bfs<V, E>(this Graph<V, E> graph, V from, Func<V, bool> goalTest)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            int u = graph.indexOfVertex(from);
            if (u != -1)
                return graph.bfs(u, goalTest);
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a breadth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter toIndex: The index of the ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> bfs<V, E>(this Graph<V, E> graph, int fromIndex, int toIndex)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            bool[] visited = Enumerable.Repeat(false, graph.vertexCount).ToArray();
            Queue<int> queue = new Queue<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            queue.Enqueue(fromIndex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                if (v == toIndex)
                    return pathDictToPath(fromIndex, toIndex, pathDict);

                foreach (E edge in graph.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        visited[edge.v] = true;
                        queue.Enqueue(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a breadth-first search.
        /// - Parameters:
        /// - parameter from: The starting vertex.
        /// - parameter to: The ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public static IEnumerable<E> bfs<V, E>(this Graph<V, E> graph, V from, V to)
            where V : IEquatable<V> where E : IEdge<E>, IEquatable<E>, new()
        {
            int u = graph.indexOfVertex(from);
            if (u != -1)
            {
                int v = graph.indexOfVertex(to);
                if (v != -1)
                    return graph.bfs(u, v);
            }
            return Enumerable.Empty<E>();
        }

        #endregion


        /// Takes a dictionary of edges to reach each node and returns an array of edges
        /// that goes from `from` to `to`
        public static List<E> pathDictToPath<E>(int from, int to, Dictionary<int, E> pathDict) where E : IEdge<E>, IEquatable<E>, new()
        {
            if (pathDict.Count == 0)
                return null;

            List<E> edgePath = new List<E>();
            if (pathDict.TryGetValue(to, out E edge))
            {
                edgePath.Add(edge);
                while (edge.u != from)
                {
                    if (pathDict.TryGetValue(edge.u, out E nextEdge))
                    {
                        edge = nextEdge;
                        edgePath.Add(edge);
                    }
                    else
                        return null;
                }
            }
            return null;
        }

    }
    public static class Cycle
    {
        /*
        /// Functions for finding cycles in a `Graph`
// MARK: Extension to `Graph` for detecting cycles
public extension Graph {
    // Based on an algorithm developed by Hongbo Liu and Jiaxin Wang
    // Liu, Hongbo, and Jiaxin Wang. "A new way to enumerate cycles in graph."
    // In Telecommunications, 2006. AICT-ICIW'06. International Conference on Internet and
    // Web Applications and Services/Advanced International Conference on, pp. 57-57. IEEE, 2006.
    
    /// Find all of the cycles in a `Graph`, expressed as vertices.
    ///
    /// - parameter upToLength: Does the caller only want to detect cycles up to a certain length?
    /// - returns: a list of lists of vertices in cycles
    func detectCycles(upToLength maxK: Int = Int.max) -> [[V]] {
        var cycles = [[V]]() // store of all found cycles
        var openPaths: [[V]] = vertices.map{ [$0] } // initial open paths are single vertex lists
        
        while openPaths.count > 0 {
            let openPath = openPaths.removeFirst() // queue pop()
            if openPath.count > maxK { return cycles } // do we want to stop at a certain length k
            if let tail = openPath.last, let head = openPath.first, let neighbors = neighborsForVertex(tail) {
                for neighbor in neighbors {
                    if neighbor == head {
                        cycles.append(openPath + [neighbor]) // found a cycle
                    } else if !openPath.contains(neighbor) && indexOfVertex(neighbor)! > indexOfVertex(head)! {
                        openPaths.append(openPath + [neighbor]) // another open path to explore
                    }
                }
            }
        }
        
        return cycles
    }

    typealias Path = [E]
    typealias PathTuple = (start: Int, path: Path)
    /// Find all of the cycles in a `Graph`, expressed as edges.
    ///
    /// - parameter upToLength: Does the caller only want to detect cycles up to a certain length?
    /// - returns: a list of lists of edges in cycles
    func detectCyclesAsEdges(upToLength maxK: Int = Int.max) -> [[E]] {

        var cycles = [[E]]() // store of all found cycles
        var openPaths: [PathTuple] = (0..<vertices.count).map{ ($0, []) } // initial open paths start at a vertex, and are empty
        while openPaths.count > 0 {
            let openPath = openPaths.removeFirst() // queue pop()
            if openPath.path.count > maxK { return cycles } // do we want to stop at a certain length k
            let tail = openPath.path.last?.v ?? openPath.start
            let head = openPath.start
            let neighborEdges = edgesForIndex(tail)
            for neighborEdge in neighborEdges {
                if neighborEdge.v == head {
                    cycles.append(openPath.path + [neighborEdge]) // found a cycle
                } else if !openPath.path.contains(where: { $0.u == neighborEdge.v || $0.v == neighborEdge.v }) && neighborEdge.v > head {
                    openPaths.append((openPath.start, openPath.path + [neighborEdge])) // another open path to explore
                }
            }
        }

        return cycles
    }
}
        */
    }
}
