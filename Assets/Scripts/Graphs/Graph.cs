using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
public class Graph{

	public HashSet<Vertex> graph = new HashSet<Vertex>();
	public List<Vertex> reBuildPath = new List<Vertex>();

	//Add a vertex to the graph
    public void AddVertex(Vertex newVertex) {
		if(!graph.Contains(newVertex)){
            graph.Add(newVertex);
        }
    }

	// A* algorithm
	public bool AStar(Vertex nVertex, Vertex goal) {
		List<Vertex> frontier = new List<Vertex>(); //Frontier, this is the vertices to explore //* Better as a Priority Queue
		List<Vertex> visited = new List<Vertex>(); // Explored

		frontier.Add(nVertex);
		nVertex.pathCost = 0;
		nVertex.heuristicCost = Distance(nVertex,goal);
		nVertex.f = nVertex.pathCost + nVertex.heuristicCost; // f = pathCost + heuristicCost

		// While not Empty
		while(frontier.Count > 0){
			int idx = MinorF(frontier);
			Vertex current = frontier[idx];
			frontier.Remove(current);
			visited.Add(current);

			// We reach the goal vertex
			if(current.id == goal.id){
				BuildPath(nVertex,goal);
				return true;
			}

			foreach(Vertex neigtbour in current.neigbours){
				if(visited.Contains(neigtbour)){
					continue;
				}

				float transitionCost = current.pathCost + 1;

				if(transitionCost < neigtbour.pathCost || !frontier.Contains(neigtbour)){
					neigtbour.pathCost = transitionCost;
					neigtbour.heuristicCost = Distance(neigtbour,goal);
					neigtbour.f = neigtbour.pathCost+neigtbour.heuristicCost;
					neigtbour.pathFather = current;

					if (!frontier.Contains(neigtbour)){
						frontier.Add(neigtbour);
					}
				}
			}
		}
		return false;
    }

	// Auxiliar method to reconstruct the 'reBuildPath' of A*
	// This reBuildPath will be used to return to the charging base
	private void BuildPath(Vertex start, Vertex goal) {
		reBuildPath.Clear();
		Vertex current = goal;

		while(current.id != start.id){
			reBuildPath.Add(current);
			current = current.pathFather;
		}

		reBuildPath.Add(start);
		reBuildPath.Reverse();
	}

	// This method calculates distace between two vertices using the Euclidean distance
	private float Distance(Vertex a, Vertex b) {
		float dx = Math.Abs(a.position.x - b.position.x); // Delta x
		float dz = Math.Abs(a.position.z - b.position.z); // Delta z
		float dy = Math.Abs(a.position.y - b.position.y); // Delta y
		double dist = dx*dx + dy*dy + dz*dz;
		return (float)Math.Sqrt(dist);
	}

	public bool IsPathEmpty() {
		return reBuildPath.Count <= 0;
	}

	// Finds the lowest F in the list, where F = G + H
	int MinorF(List<Vertex> list) {
		float minF = list[0].f;
		float minH = list[0].heuristicCost;
		int idx = 0;
		for(int i = 0; i < list.Count; i++){
			if(list[i].f < minF || (list[i].f == minF && list[i].heuristicCost < minH)){
				minF = list[i].f;
				minH = list[i].heuristicCost;
				idx = i;
			}
		}

		return idx;
	}
}
