using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batery : MonoBehaviour
{
    public float batery;
    public float maxCapacity;
    public float chargeSpeed; 

    // Drains batery at a rate of 1 unit per second
    void Update(){
        if(batery > 0) 
            batery -= Time.deltaTime;
    }

    // Charge the batery at a rate of 'chargeSpeed' units per second
    public void Charge(){
        if(batery < maxCapacity)
            batery += Time.deltaTime * chargeSpeed;
    }

    public void SetChargeSpeed(float velocity){
        chargeSpeed = velocity;
    }

    // Directly charges the batery at its max capacity
    // Should be used once at the start of the game
    // ! Who wants a game with a half-empty batery?
    public void FullyCharge(){
        batery = maxCapacity;
    }

    // Returns the current batery level
    public float BateryLevel(){
        return batery;
    }
}
