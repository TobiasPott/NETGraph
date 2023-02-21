using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph
{

    public abstract partial class Graph<V, E>
    {

        public enum SearchAlgorithm
        {
            DepthFirst,
            BreadthFirst
        }

        public int search(int fromIndex, Func<int, bool> goalTest, Func<IEnumerable<E>, IEnumerable<E>> visitOrder, Func<E, bool> reducer, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.dfs(fromIndex, goalTest, visitOrder, reducer);
                case SearchAlgorithm.BreadthFirst:
                    return this.bfs(fromIndex, goalTest, visitOrder, reducer);
            }
            return -1;
        }
        public IEnumerable<E> search(int fromIndex, Func<V, bool> goalTest, SearchAlgorithm algorithm)
        {

            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.dfs(fromIndex, goalTest);
                case SearchAlgorithm.BreadthFirst:
                    return this.bfs(fromIndex, goalTest);
            }
            return Enumerable.Empty<E>();
        }
        public IEnumerable<E> search(V from, Func<V, bool> goalTest, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.dfs(from, goalTest);
                case SearchAlgorithm.BreadthFirst:
                    return this.bfs(from, goalTest);
            }
            return Enumerable.Empty<E>();
        }
        public IEnumerable<E> search(int fromIndex, int toIndex, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.dfs(fromIndex, toIndex);
                case SearchAlgorithm.BreadthFirst:
                    return this.bfs(fromIndex, toIndex);
            }
            return Enumerable.Empty<E>();
        }
        public IEnumerable<E> search(V from, V to, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.dfs(from, to);
                case SearchAlgorithm.BreadthFirst:
                    return this.bfs(from, to);
            }
            return Enumerable.Empty<E>();
        }

        public List<List<E>> findAll(int fromIndex, Func<V, bool> goalTest, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.findAllDfs(fromIndex, goalTest);
                case SearchAlgorithm.BreadthFirst:
                    return this.findAllBfs(fromIndex, goalTest);
            }
            return new List<List<E>>();
        }
        public List<List<E>> findAll(V from, Func<V, bool> goalTest, SearchAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case SearchAlgorithm.DepthFirst:
                    return this.findAllDfs(from, goalTest);
                case SearchAlgorithm.BreadthFirst:
                    return this.findAllBfs(from, goalTest);
            }
            return new List<List<E>>();
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
        public int dfs(int fromIndex, Func<int, bool> goalTest, Func<IEnumerable<E>, IEnumerable<E>> visitOrder, Func<E, bool> reducer)
        {
            if (goalTest(fromIndex))
                return fromIndex;

            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Stack<E> stack = new Stack<E>();

            visited[fromIndex] = true;
            IEnumerable<E> neighbours = this.edgesForIndex(fromIndex);
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
                    IEnumerable<E> edgeNeighbours = this.edgesForIndex(v);
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
        public IEnumerable<E> dfs(int fromIndex, Func<V, bool> goalTest)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
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
                if (goalTest(this.vertexAtIndex(v)))
                    return pathDictToPath(fromIndex, v, pathDict);

                foreach (E edge in this.edgesForIndex(v))
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
        public IEnumerable<E> dfs(V from, Func<V, bool> goalTest)
        {
            int fromIndex = this.indexOfVertex(from);
            if (fromIndex != -1)
                return this.dfs(fromIndex, goalTest);
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a depth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter toIndex: The index of the ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public IEnumerable<E> dfs(int fromIndex, int toIndex)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
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

                foreach (E edge in this.edgesForIndex(v))
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
        public IEnumerable<E> dfs(V from, V to)
        {
            int fromIndex = this.indexOfVertex(from);
            if (fromIndex != -1)
            {
                int toIndex = this.indexOfVertex(to);
                if (toIndex != -1)
                    return this.dfs(fromIndex, toIndex);
            }
            return Enumerable.Empty<E>();
        }

        /// Find path routes from a vertex to all others the function goalTest() returns true for using a depth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of arrays of Edges containing routes to every vertex connected and passing goalTest(), or an empty array if no routes could be found
        public List<List<E>> findAllDfs(int fromIndex, Func<V, bool> goalTest)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Stack<int> stack = new Stack<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            List<List<E>> paths = new List<List<E>>();
            stack.Push(fromIndex);

            while (stack.Count > 0)
            {
                int v = stack.Pop();
                if (visited[v])
                    continue;

                visited[v] = true;
                // return path if index fullfills goalTest delegate
                if (goalTest(this.vertexAtIndex(v)))
                    paths.Add(pathDictToPath(fromIndex, v, pathDict));

                foreach (E edge in this.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        stack.Push(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return paths;
        }

        /// Find path routes from a vertex to all others the function goalTest() returns true for using a depth-first search.
        /// - Parameters:
        /// - parameter from: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of arrays of Edges containing routes to every vertex connected and passing goalTest(), or an empty array if no routes could be founding the entire route, or an empty array if no route could be found
        public List<List<E>> findAllDfs(V from, Func<V, bool> goalTest)
        {
            int fromIndex = indexOfVertex(from);
            if (fromIndex > 0)
                return findAllDfs(fromIndex, goalTest);
            return new List<List<E>>();
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
        public int bfs(int fromIndex, Func<int, bool> goalTest, Func<IEnumerable<E>, IEnumerable<E>> visitOrder, Func<E, bool> reducer)
        {
            if (goalTest(fromIndex))
                return fromIndex;

            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Queue<E> queue = new Queue<E>();

            visited[fromIndex] = true;
            IEnumerable<E> neighbours = this.edgesForIndex(fromIndex);
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
                    IEnumerable<E> edgeNeighbours = this.edgesForIndex(v);
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
        public IEnumerable<E> bfs(int fromIndex, Func<V, bool> goalTest)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Queue<int> queue = new Queue<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            queue.Enqueue(fromIndex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                visited[v] = true;
                // return path if index fullfills goalTest delegate
                if (goalTest(this.vertexAtIndex(v)))
                    return pathDictToPath(fromIndex, v, pathDict);

                foreach (E edge in this.edgesForIndex(v))
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
        public IEnumerable<E> bfs(V from, Func<V, bool> goalTest)
        {
            int fromIndex = this.indexOfVertex(from);
            if (fromIndex != -1)
                return this.bfs(fromIndex, goalTest);
            return Enumerable.Empty<E>();
        }

        /// Find a route from one vertex to another using a breadth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter toIndex: The index of the ending vertex.
        /// - returns: An array of Edges containing the entire route, or an empty array if no route could be found
        public IEnumerable<E> bfs(int fromIndex, int toIndex)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Queue<int> queue = new Queue<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            queue.Enqueue(fromIndex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                if (v == toIndex)
                    return pathDictToPath(fromIndex, toIndex, pathDict);

                foreach (E edge in this.edgesForIndex(v))
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
        public IEnumerable<E> bfs(V from, V to)
        {
            int fromIndex = this.indexOfVertex(from);
            if (fromIndex != -1)
            {
                int toIndex = this.indexOfVertex(to);
                if (toIndex != -1)
                    return this.bfs(fromIndex, toIndex);
            }
            return Enumerable.Empty<E>();
        }


        /// Find path routes from a vertex to all others the function goalTest() returns true for using a breadth-first search.
        /// - Parameters:
        /// - parameter fromIndex: The index of the starting vertex.
        /// - parameter goalTest: Returns true if a given vertex is a goal.
        /// - returns: An array of arrays of Edges containing routes to every vertex connected and passing goalTest(), or an empty array if no routes could be found
        public List<List<E>> findAllBfs(int fromIndex, Func<V, bool> goalTest)
        {
            bool[] visited = Enumerable.Repeat(false, this.vertexCount).ToArray();
            Queue<int> queue = new Queue<int>();
            Dictionary<int, E> pathDict = new Dictionary<int, E>();
            List<List<E>> paths = new List<List<E>>();
            queue.Enqueue(fromIndex);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                if (goalTest(this.vertexAtIndex(v)))
                    paths.Add(pathDictToPath(fromIndex, v, pathDict));

                foreach (E edge in this.edgesForIndex(v))
                {
                    if (!visited[edge.v])
                    {
                        visited[edge.v] = true;
                        queue.Enqueue(edge.v);
                        pathDict.TryAdd(edge.v, edge);
                    }
                }
            }
            return paths;
        }

        public List<List<E>> findAllBfs(V from, Func<V, bool> goalTest)
        {
            int fromIndex = this.indexOfVertex(from);
            if (fromIndex != -1)
                return this.findAllBfs(fromIndex, goalTest);
            return new List<List<E>>();
        }

        #endregion


        /// Takes a dictionary of edges to reach each node and returns an array of edges
        /// that goes from `from` to `to`
        public static List<E> pathDictToPath(int from, int to, Dictionary<int, E> pathDict)
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

}

