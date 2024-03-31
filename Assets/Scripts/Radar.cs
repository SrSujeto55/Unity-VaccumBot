using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Componente auxiliar que utiliza un Collider esférico a manera de radar
// para comprobar colisiones con otros elementos.
// Las comprobaciones y métodos son análogos al componente (script) de Sensores.
public class Radar : MonoBehaviour{

    private bool closeToTrash;
    private GameObject trash;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Trash")){
            closeToTrash = true;
            trash = other.gameObject;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Trash")){
            closeToTrash = true;
            trash = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Trash")){
            closeToTrash = false;
        }
    }

    public bool CloseToTrash(){
        return closeToTrash;
    }

    public GameObject GetTrash(){
        return trash;
    }

    public void setTouchingTrash(bool value){
        closeToTrash = value;
    }

    public void setCloseToTrash(bool value){
        closeToTrash = value;
    }
}
