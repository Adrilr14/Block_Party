using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    //public GameObject posDestino;
    public GameObject[] posiblesPosiciones; //array para guardar todas las posiciones posibles del tablero, metiendo los objetos de las casillas
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moverCoche(int numDado, GameObject coche, int pos)
    {
        StartCoroutine(LerpPosition(numDado, coche, pos));
    }
    public IEnumerator LerpPosition(int numDado, GameObject coche, int pos)
    {
        float time = 0;
        Vector3 startPosition = coche.transform.position;
        Vector3 targetPosition = posiblesPosiciones[pos].transform.GetChild(0).gameObject.transform.position;
        while (time < .8f)
        {
            coche.transform.position = Vector3.Lerp(startPosition, targetPosition, time / .8f);
            time += Time.deltaTime;
            yield return null;
        }
        coche.transform.position = targetPosition;
        StartCoroutine(LerpRotation(numDado, coche, pos));    
    }

    public IEnumerator LerpRotation(int numDado, GameObject coche, int pos)
    {
        float time = 0;
        Quaternion startRotation = coche.transform.rotation;
        Quaternion targetRotation = posiblesPosiciones[pos].transform.GetChild(0).gameObject.transform.rotation;
        if(startRotation != targetRotation) // Poner aquí punto para debuggear
        {
            while (time < .8f)
            {
                coche.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / .8f);
                time += Time.deltaTime;
                yield return null;
            }
        }
        coche.transform.rotation = targetRotation;
        if (numDado == 0)
        {
            gManager.comprobarCasilla(posiblesPosiciones[pos].transform.gameObject.tag, pos);
        }
        else
        {
            if (posiblesPosiciones[pos].transform.gameObject.tag == "ultimaCasilla")
                gManager.comprobarCasilla(posiblesPosiciones[pos].transform.gameObject.tag, pos);
            else
                moverCoche(numDado - 1, coche, pos + 1);
        }
    }
}
