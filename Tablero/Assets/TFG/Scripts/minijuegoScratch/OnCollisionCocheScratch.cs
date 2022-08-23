using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionCocheScratch : MonoBehaviour
{
    public GameManagerScratch gms;

    //Detecta las colisiones de los coches (al principio de la colisión)
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.CompareTag("rueda"))
        {
            gms.Ruedas++;
            gms.ruedasCogidasList.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
