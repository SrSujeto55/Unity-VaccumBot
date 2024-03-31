using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticBehaviour : MonoBehaviour {


    // All posible states of the bot
    public enum State {
        MAPPING, // To create a map of the environment
        DFS, // To explore the environment
        COMEBACK, // To return the last Stack Vertex
        REST, // To rest c:
        CHARGING, // To charge batery
        RETURNBASE, // To return to the base
        RETURNFROMBASE // To return to the last Vertex after charging
    }

    public State currentState;
    private Sensors sensor;
	private Actuators actuators;
	private Map map;
    public Vertex origin, last, destiny;
    public bool fp, ChangeDirection; // The FP variable is used to prevent to pop all vertex when thay are not required 
    private int pathIndex = 0;
    private State prevState;
    public List<Vertex> pathToBase = new List<Vertex>();
    public Vertex currentPath;

    // Start is called before the first frame update
    void Start(){
        fp = true;
        map = GetComponent<Map>();
        sensor = GetComponent<Sensors>();
		actuators = GetComponent<Actuators>();

        map.SetNode(0);
        map.PopStack(out destiny);
        last = destiny; 
        map.PushStack(last);
        origin = last;
        map.SetPrevious(last);
        SetState(State.DFS); 
    }

    // Updates the bot state
    void FixedUpdate() {
        if((currentState == State.MAPPING || currentState == State.DFS || currentState == State.COMEBACK) && !EnoughBatery()){
            prevState = currentState;
            ChangeDirection = false;
            SetState(State.RETURNBASE);
        }

        if(sensor.CloseToTrash()){
            actuators.CleanUp(sensor.GetTrash());
        }

        switch (currentState) {
            case State.MAPPING: 
                UpdateMapping();
                break;
            case State.REST:
                actuators.Stop();
                break;
            case State.DFS:
                UpdateDFS();
                break;
            case State.COMEBACK:
                RetrunToStackDestiny();
                break;
            case State.RETURNBASE:
                ReturnToBase();
                break;
            case State.CHARGING:
                ChargeBatery();
                break;
            case State.RETURNFROMBASE:
                ReturnFromBase();
                break;
        }
    }

    // Method to update the mapping of the environment
    void UpdateMapping() {
        if(fp){ 
            map.PopStack(out destiny);
            map.SetPrevious(destiny);
            fp = false;
        }
        if(destiny != null){
            if(destiny.father != last){
                SetState(State.COMEBACK);
                return;
            }
            if(Vector3.Distance(sensor.Ubication(), destiny.position) >= 0.04f){
                if(!ChangeDirection){
                    this.transform.LookAt(destiny.position);
                    ChangeDirection = true;
                }   
                actuators.Forward();
            }else{
                last = destiny;
                ChangeDirection = false;
                fp = true;
                SetState(State.DFS);
            }
        }else{
            SetState(State.REST);
        }

    }

    // Method to return last visited vertex in the DFS stack
    // We do this based on the fact that the sub-tree is linked with a parent-child relationship, so we return to the parent of each node
    // until that parent is the node we need from the DFS stack 
    void RetrunToStackDestiny(){
        if(last.father != null){
            if(Vector3.Distance(sensor.Ubication(), last.father.position) >= 0.04f){
                if(!ChangeDirection){
                    this.transform.LookAt(last.father.position);
                    ChangeDirection = true;
                }  
                actuators.Forward(); 
            }else{
                last = last.father;
                ChangeDirection = false;
                fp = false;
                SetState(State.MAPPING);
            }
        }else{
            SetState(State.REST);
        }
    }

    // Method to check if the batery level is over a certain value (40% by default)
    bool EnoughBatery(int bateryLevel = 40){
        return sensor.GetBateryLevel() > bateryLevel;
    }

    // Method to return base with A* algorithm
    void ReturnToBase(){
        if(pathToBase.Count == 0){
            if(map.tryAStar(last, map.changingBase)){
                pathToBase = map.GetAStarPath();
            }else{
                print("The path to the base could not be found, entering safe mode");
                SetState(State.REST);
            }
        }else{
            if(pathIndex != pathToBase.Count){
                if (Vector3.Distance(sensor.Ubication(), pathToBase[pathIndex].position) >= 0.04f)
                { 
                    transform.LookAt(pathToBase[pathIndex].position);
                    actuators.Forward();
                }
                else
                { 
                    currentPath = pathToBase[pathIndex];
                    pathIndex++;
                }
            }else{
                SetState(State.CHARGING); // We reach our goal
            }

        }
    }

    // Method to charge the batery, every time it's called, it charges a certain amount based on the charging speed
    // It ends when the batery is full, in which case, it returns to the last visited node before going to charge the batery
    void ChargeBatery(){
        print(sensor.GetBateryLevel());
        if(sensor.GetBateryLevel() < sensor.GetMaxBatery()-1){
            actuators.ChargeBatery();
        }else{
            SetState(State.RETURNFROMBASE);
        }
    }

    // Method to return to the last visited node before going to charge the batery
    void ReturnFromBase(){
        if(pathIndex != -1){ // While the vertex is not the last we visited
            if(pathIndex == pathToBase.Count){
                pathIndex--;
            }

            if (Vector3.Distance(sensor.Ubication(), pathToBase[pathIndex].position) >= 0.04f)
            {
                transform.LookAt(pathToBase[pathIndex].position);
                actuators.Forward();
            }
            else
            {
                currentPath = pathToBase[pathIndex];
                pathIndex--;
            }
        }else{
            pathIndex = 0;
            pathToBase = new List<Vertex>();
            currentPath = null;
            map.ClearPath();
            SetState(prevState);
        }
    }

    // Method to add new vertex to the DFS stack if the sensors detect a free path
    // The order is Left, Right, Front, so the bot always tries to go forward when popping a vertex
    void UpdateDFS(){
        if(sensor.FreeLeft()){
            map.SetNode(1);
        }
        if(sensor.FreeRight()){
            map.SetNode(3);
        }
        if(sensor.FreeFront()){
            map.SetNode(2);
        }
        SetState(State.MAPPING);
    }

    // Method to set the current state of the bot
    void SetState(State newState) {
        currentState = newState;
    }

}
