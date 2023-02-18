using System;
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
    //  *   [+] Reversed
    //  *   [ ] Search || DONE
    //  *   [ ] Sort
    //  *   [ ] Union
    //  *   
    //  *

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
