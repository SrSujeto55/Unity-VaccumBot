using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Componente auxiliar para genera rayos que detecten colisiones de manera lineal
// En el script actual dibuja y comprueba colisiones con un rayo al frente del objeto
// y a un costado, sin embargo, es posible definir más rayos de la misma manera.
public class Ray : MonoBehaviour{

    public float rayLenght,radio;
    public LayerMask obstacles;
    private bool freeFront, freeLeft, freeRight;


    void Update(){
        // Draw the rays in the scene view
        Debug.DrawLine(transform.position, transform.position + (transform.forward * rayLenght), Color.red);
        Debug.DrawLine(transform.position, transform.position + (transform.right * rayLenght), Color.blue);
        Debug.DrawLine(transform.position, transform.position + (transform.right*-1 * rayLenght), Color.blue);
    }

    // Try to detect collisions with the rays near the object
    void FixedUpdate(){
        freeFront = true;
        freeLeft = true;
        freeRight = true;
        RaycastHit raycastFront;
        if (Physics.Raycast(transform.position, transform.forward, out raycastFront, rayLenght, obstacles)) {
            freeFront = false;
        }
        RaycastHit raycastLeft;
        if (Physics.Raycast(transform.position, transform.right * -1, out raycastLeft, rayLenght, obstacles)) {
            freeLeft = false;
        }
        RaycastHit raycastRigth;
        if (Physics.Raycast(transform.position, transform.right, out raycastRigth, rayLenght, obstacles)) {
            freeRight = false;
        }
        Collider[] fColliders = Physics.OverlapSphere(transform.position + (transform.forward * rayLenght), radio, obstacles);
        if (fColliders.Length > 0) {
            freeFront = false;
        }
        Collider[] iColliders = Physics.OverlapSphere(transform.position + (transform.right * -1 * rayLenght), radio, obstacles);
        if (iColliders.Length > 0) {
            freeLeft = false;
        }
        Collider[] dColliders = Physics.OverlapSphere(transform.position + (transform.right * rayLenght), radio, obstacles);
        if (dColliders.Length > 0) {
            freeRight = false;
        }
    }

    public bool FreeFront(){
        return freeFront;
    }
    public bool FreeLeft() {
        return freeLeft;
    }
    public bool FreeRight() {
        return freeRight;
    }
}
