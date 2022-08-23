using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    // This will not show in the inspector

    [HideInInspector]
    public Collision collision;

    //cuando empieza a colisionar
    void OnCollisionEnter (Collision collision)
    {
        //Debug.Log("Enter called.");

    }

    // cuando esta colisionando
    void OnCollisionStay (Collision collision)
    {
        //Debug.Log("Stay occuring...");
    }

    // comprueba que ha dejado de colisionar
    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exit called...");
    }
}
