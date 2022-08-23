using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarCamaraMapa : MonoBehaviour
{

    public static bool MapaIsActive;

    public int lastEstadoJuego; // Variable para guardar el estado del juego antes de activar el mapa

    //scripts
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        MapaIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && (gm.estadoJuego == 0 || gm.estadoJuego == 1 || gm.estadoJuego == 4 || gm.estadoJuego == 5) && !PauseMenu.GameIsPaused)
        {
            if (MapaIsActive)
            {
                CerrarMapa();
            }
            else
            {
                AbrirMapa();
            }
        }
    }

    private void AbrirMapa()
    {
        MapaIsActive = true;
        gm.camaraMapa.enabled = true;
        lastEstadoJuego = gm.estadoJuego;
        gm.estadoJuego = 5;
        Time.timeScale = 0f; // Parar la ejecución del juego
        gm.canvasPuntos.gameObject.SetActive(false); // Desactivamos el canvas de los puntos
    }

    private void CerrarMapa()
    {
        MapaIsActive = false;
        gm.camaraMapa.enabled = false;
        gm.estadoJuego = lastEstadoJuego;
        Time.timeScale = 1f; // Activar la ejecución del juego
        gm.canvasPuntos.gameObject.SetActive(true); // Activamos el canvas de los puntos
    }
}
