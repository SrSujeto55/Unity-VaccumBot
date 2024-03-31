using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour{

    private Radar radar; 
    private Ray rayCasting;
    private Batery batery; 
    public bool closeToTrash = false;
    
    void Start(){
        radar = GameObject.Find("Radar").gameObject.GetComponent<Radar>();
        rayCasting = GameObject.Find("Ray").gameObject.GetComponent<Ray>();
        batery = GameObject.Find("Batery").gameObject.GetComponent<Batery>();
    }

    public bool FreeFront(){
        return rayCasting.FreeFront();
    }

    public bool FreeLeft() {
        return rayCasting.FreeLeft();
    }

    public bool FreeRight() {
        return rayCasting.FreeRight();
    }

    public bool CloseToTrash(){
        return radar.CloseToTrash();
    }

    public GameObject GetTrash(){
        return radar.GetTrash();
    }

    public Vector3 Ubication(){
        return transform.position;
    }

    public float GetBateryLevel(){
        return batery.BateryLevel();
    }

    public float GetMaxBatery(){
        return batery.maxCapacity;
    }

    public void SetCloseToTrash(bool value){
        radar.setCloseToTrash(value);
    }

    public void SetTouchingTrash(bool value){
        radar.setTouchingTrash(value);
    }
}
