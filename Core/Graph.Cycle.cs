using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{

    public abstract partial class Graph<V, E>
    {

        /// Find all of the cycles in a `Graph`, expressed as edges.
        ///
        /// - parameter upToLength: Does the caller only want to detect cycles up to a certain length?
        /// - returns: a list of lists of edges in cycles
        public List<List<V>> detectCycles(int upToLength = int.MaxValue)
        {
            List<List<V>> cycles = new List<List<V>>();
            Queue<List<V>> openPaths = new Queue<List<V>>(this.vertices.Select(v => new List<V>() { v }));

            while (openPaths.Count > 0)
            {
                List<V> openPath = openPaths.Dequeue();
                if (openPath.Count > upToLength)
                    return cycles;

                if (openPath.Count > 0)
                {
                    V tail = openPath[openPath.Count - 1];
                    V head = openPath[0];
                    IEnumerable<V> neighbours = neighborsForVertex(tail);
                    foreach (V neighbour in neighbours)
                    {
                        if (neighbour.Equals(head))
                            cycles.Add(openPath.Append(neighbour).ToList());
                        else if (!openPath.Contains(neighbour) && indexOfVertex(neighbour) > indexOfVertex(head))
                            openPaths.Enqueue(openPath.Append(neighbour).ToList());
                    }

                }

            }

            return cycles;
        }

        /// Find all of the cycles in a `Graph`, expressed as edges.
        ///
        /// - parameter upToLength: Does the caller only want to detect cycles up to a certain length?
        /// - returns: a list of lists of edges in cycles
        public List<List<E>> detectCyclesAsEdges(int upToLength = int.MaxValue)
        {

            List<List<E>> cycles = new List<List<E>>();
            Queue<(int start, List<E> path)> openPaths = new Queue<(int, List<E>)>(Enumerable.Range(0, this.vertices.Count).Select(x => (x, new List<E>())));

            while (openPaths.Count > 0)
            {
                (int start, List<E> path) openPath = openPaths.Dequeue();
                List<E> path = openPath.path;

                if (path.Count > upToLength)
                    return cycles;

                int tail = path.Count > 0 ? path.Last().v : openPath.start;
                int head = openPath.start;

                IEnumerable<E> neighbourEdges = edgesForIndex(tail);
                foreach (E neighbourEdge in neighbourEdges)
                {
                    if (neighbourEdge.v == head)
                        cycles.Add(path.Append(neighbourEdge).ToList());
                    else if (!path.Any(e => e.u == neighbourEdge.v || e.v == neighbourEdge.v || neighbourEdge.v > head))
                        openPaths.Enqueue((openPath.start, path.Append(neighbourEdge).ToList()));

                }

            }

            return cycles;
        }

    }
}
