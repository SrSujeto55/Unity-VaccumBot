using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase implementada por Link! 
public class Map : MonoBehaviour{

    public Transform graph;
    public Graph graphMap = new Graph();
    public Vertex previous = null, auxiliarVertex, found = null;
    public GameObject nodo, newNodo;
    public int numberOfNodes = -1;
    public bool canSetNode = true;
    public float threShold;
    Vector3 offsetAux;
    public Vertex changingBase;
    public Stack<Vertex> dfsStack = new Stack<Vertex>();

    // Keeps track of the whole graph in real time
    // tracks a path from the start to the goal when needed
    void Update(){
        DrawGraph();
        if (!graphMap.IsPathEmpty()) {
            DrawPath();
        }
    }

     // Places a node where it is needed
        // 0 at the center of the object
        // 1 to the left
        // 2 to the front
        // 3 to the right
    // The position is determined with an offset of "threShold"
    // If there is already a vertex in that position, it does not place it and makes it adjacent to the existing one.
    public void SetNode(int direction) {
        if (!IsMapEmpty()) {
            switch (direction) {
                case 0: // Center
                foreach (Vertex v in graphMap.graph) {
                    if (Vector3.Distance(this.gameObject.transform.position, v.position) < threShold) {
                        canSetNode = false;
                        break;
                    }
                }
                break;
                case 1: // Left
                offsetAux = transform.position + this.transform.right * -1 * threShold;
                foreach (Vertex v in graphMap.graph) {
                    if (Vector3.Distance(offsetAux, v.position) < threShold - 0.15) { // Checks if a vertex is close enough to the new one
                        found = v;
                        canSetNode = false;
                        break;
                    }
                }
                break;
                case 2: // Front
                offsetAux = transform.position + this.transform.forward * threShold;
                foreach (Vertex v in graphMap.graph) {
                    if (Vector3.Distance(offsetAux, v.position) < threShold - 0.15) { // Checks if a vertex is close enough to the new one
                        found = v;
                        canSetNode = false;
                        break;
                    }
                }
                break;
                case 3: // Right
                offsetAux = transform.position + this.transform.right  * threShold;
                foreach (Vertex v in graphMap.graph) {
                    if (Vector3.Distance(offsetAux, v.position) < threShold - 0.15) { // Checks if a vertex is close enough to the new one
                        found = v; 
                        canSetNode = false;
                        break;
                    }
                }
                break;
            }
        }
        if (canSetNode) {
            numberOfNodes++;
            switch (direction) {
                case 0: // Center, Only happens once
                newNodo = Instantiate(nodo, this.gameObject.transform.position, Quaternion.identity); // Creates a visible node in the map
                newNodo.transform.SetParent(graph);
                auxiliarVertex = new Vertex(numberOfNodes, this.transform.position);
                previous = auxiliarVertex;   
                changingBase = auxiliarVertex;             
                break;
                case 1: // Left
                offsetAux = transform.position + this.transform.right * -1 * threShold;
                newNodo = Instantiate(nodo, offsetAux, Quaternion.identity);
                newNodo.transform.SetParent(graph);
                auxiliarVertex = new Vertex(numberOfNodes, offsetAux);
                break;
                case 2: // Front
                offsetAux = transform.position + this.transform.forward * threShold;
                newNodo = Instantiate(nodo, offsetAux, Quaternion.identity);
                newNodo.transform.SetParent(graph);
                auxiliarVertex = new Vertex(numberOfNodes, offsetAux);
                break;
                case 3: //Right
                offsetAux = transform.position + this.transform.right * threShold;
                newNodo = Instantiate(nodo, offsetAux, Quaternion.identity);
                newNodo.transform.SetParent(graph);
                auxiliarVertex = new Vertex(numberOfNodes, offsetAux);
                break;
            }
            dfsStack.Push(auxiliarVertex);

            if (!IsMapEmpty()) {
                previous.AddNeigbour(auxiliarVertex);
                auxiliarVertex.AddNeigbour(previous);
                auxiliarVertex.setFather(previous);
            }

            newNodo.GetComponent<Tag>().setId(numberOfNodes);
            graphMap.AddVertex(auxiliarVertex);
            found = null;
        }
        if (found != null) {
            previous.AddNeigbour(found);
            found.AddNeigbour(previous);
        }
        canSetNode = true;
    }

    public void SetPrevious(Vertex newPreV) {
        previous = newPreV;
    }

    public bool IsStackEmpty() {
        return dfsStack.Count == 0;
    }

    public bool PopStack(out Vertex stack) {
        stack = null;
        return dfsStack.TryPop(out stack);
    }

    public void PushStack(Vertex vert){
        dfsStack.Push(vert);
    }

    // Debugs the graph directly in the scene
    public void DrawGraph() {
        if (numberOfNodes >= 0) {
            foreach (Vertex g in graphMap.graph) {
                foreach (Vertex v in g.neigbours) {
                    Debug.DrawLine(g.position, v.position, Color.red);
                }
            }
        }
    }

    // Debugs the path directly in the scene
    public void DrawPath() {
        if (graphMap.reBuildPath != null && !graphMap.IsPathEmpty()) {
            List<Vertex> auxVertex = graphMap.reBuildPath;
        for (int i = auxVertex.Count - 1; i > 0; i--) {
            if (auxVertex[i] != null && auxVertex[i].pathFather != null) {
                Debug.DrawLine(auxVertex[i].position, auxVertex[i].pathFather.position, Color.green);
            } else {
                Debug.LogWarning("Null reference in path.");
            }
        }
        } else {
            Debug.LogWarning("Empty Path");
        }
    }

    // Clears the path
    public void ClearPath() {
        graphMap.reBuildPath.Clear();
    }

    public bool IsMapEmpty() {
        return numberOfNodes <= 0;
    }

    public bool tryAStar(Vertex start, Vertex goal) {
        return graphMap.AStar(start, goal);
    }

    public List<Vertex> GetAStarPath() {
        return graphMap.reBuildPath;
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + this.transform.right * -1*threShold, 0.6f);
        Gizmos.DrawWireSphere(transform.position + this.transform.forward * threShold, 0.6f);
        Gizmos.DrawWireSphere(transform.position + this.transform.right * threShold, 0.6f);
    }

}
