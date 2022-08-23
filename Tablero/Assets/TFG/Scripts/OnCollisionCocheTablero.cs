using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionCocheTablero : MonoBehaviour
{
    public GameManager gm;

    //Detecta las colisiones de los coches (al principio de la colisión)
    void OnCollisionEnter(Collision collision)
    {
        for(int x = 0; x < gm.numJugadores; x++)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            if (collision.gameObject.name == gm.dados[x].transform.GetChild(0).name)
            {
                gm.PararDado(x); //Paramos el dado
            }
        }
    }
}
