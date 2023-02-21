using System;
using System.Collections.Generic;
using System.Linq;

namespace NETGraph
{
    // ToDo: Continue working on left out utilities

    // Stub class for missing helper and utility methods for different graph types and situations
    //  *   [ ] Sort

    public enum TSColor
    {
        black, grey, white
    }

    public abstract partial class Graph<V, E>
    {
        /// Topologically sorts a `Graph` O(n)
        ///
        /// - returns: the sorted vertices, or nil if the graph cannot be sorted due to not being a DAG
        //public List<V> topologicalSort()
        //{
        //    List<V> sortedVertices = new List<V>();
        //    IEnumerable<int> rangeOfVertices = Enumerable.Range(0, vertexCount);
        //    List<(int index, TSColor color)> tsNodes = rangeOfVertices.Select(x => (x, TSColor.black)).ToList();
        //    bool notDAG = false;

        //    // Determine vertex neighbors in advance, so we have to do it once for each node.
        //    //List<HashSet<int>> neighbours = rangeOfVertices.Select(x => edges[x]);
        //}


        /*
    func topologicalSort() -> [V]? {

        // Determine vertex neighbors in advance, so we have to do it once for each node.
        let neighbors: [Set<Int>] = rangeOfVertices.map({ index in
            Set(edges[index].map({ $0.v })) // creates Set<Int> with v index of each edge in edges dict (select
        })
        
        func visit(_ node: TSNode) {
            guard node.color != .gray else {
                notDAG = true
                return
            }
            if node.color == .white {
                node.color = .gray
                for inode in tsNodes where neighbors[node.index].contains(inode.index) {
                    visit(inode)
                }
                node.color = .black
                sortedVertices.insert(vertices[node.index], at: 0)
            }
        }
        
        for node in tsNodes where node.color == .white {
            visit(node)
        }
        
        if notDAG {
            return nil
        }
        
        return sortedVertices
    }
    
    
    /// Is the `Graph` a directed-acyclic graph (DAG)? O(n)
    /// Finds the answer based on the result of a topological sort.
    var isDAG: Bool {
        guard let _ = topologicalSort() else { return false }
        return true
    }


// MARK: Utility structures for topological sorting
fileprivate enum TSColor { case black, gray, white }

fileprivate class TSNode {
    fileprivate let index: Int
    fileprivate var color: TSColor

    init(index: Int, color: TSColor) {
        self.index = index
        self.color = color
    }
}

        */
    }
}

