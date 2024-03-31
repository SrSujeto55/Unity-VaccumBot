using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation of a vertex
// A vertex contains an ID, position and a set of neighbours
public class Vertex{

    public HashSet<Vertex> neigbours = new HashSet<Vertex>();
    public int id;
    public Vertex father;
    public Vertex pathFather;
    public Vector3 position;
    public float f; // Defined as PathCost + HeuristicCost
    public float pathCost { get; set; }
    public float heuristicCost { get; set; }

    public Vertex(int newId, Vector3 newPos) {
        this.id = newId;
        this.position = newPos;
    }

    public void setFather(Vertex father) {
        this.father = father;
    }

    public void AddNeigbour(Vertex newVertex) {
        if(!neigbours.Contains(newVertex)){
            neigbours.Add(newVertex);
        }
    }
}
