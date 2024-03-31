using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuators : MonoBehaviour{

    private Rigidbody rb;
    private Batery batery; 
    private Sensors sensor;

    public float velocity;

    void Start(){
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<Sensors>();
        batery = GameObject.Find("Batery").gameObject.GetComponent<Batery>();
        batery.FullyCharge();
        batery.SetChargeSpeed(16.0f);
    }

    // Moves the gameObject forward at a rate of velocity
    public void Forward(){
        this.gameObject.transform.Translate(0, 0, velocity * Time.deltaTime);
    }

    // Stops the gameObject Velocity
    public void Stop(){
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Deletes the gameObject set as Trash
    public void CleanUp(GameObject trash){
        trash.SetActive(false);
        sensor.SetTouchingTrash(false);
        sensor.SetCloseToTrash(false);
    }

    // set the batery to charge for a unit
    public void ChargeBatery(){
        batery.Charge();
    }
}
