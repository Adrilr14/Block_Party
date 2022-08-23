using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScreen : MonoBehaviour
{

    public GameObject initScreenUI;

    //scripts
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //initScreenUI.SetActive(true); // Activamos la pantalla de inicio al principio para tenerla desactivada al restablecer la escena
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && gm.estadoJuego == -3)
        {
            IniciarMenuPrincipal();
        }
    }

    private void IniciarMenuPrincipal()
    {
        initScreenUI.SetActive(false); // Desactivamos la pantalla de inicio
        gm.estadoJuego = -1; // Cambiamos el estado del juego al menú principal
    }
}
