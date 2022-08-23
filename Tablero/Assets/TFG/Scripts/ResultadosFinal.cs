using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultadosFinal : MonoBehaviour
{
    // Objects
    public GameObject resultadosPanel;
    public GameObject menuPanel;
    public GameObject resultadosTexts;

    // Variables
    private bool resultadosMostrados;

    // Scripts
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializamos algunas variables
        resultadosMostrados = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!resultadosMostrados && gm.estadoJuego == 7)
        {
            Time.timeScale = 0f;
            resultadosMostrados = true;
            mostrarResultados();
        }
    }

    private void mostrarResultados()
    {
        // Guardamos los puntos ordenados de mayor a menor
        List<int> listaPuntosOrdenada = new List<int>(gm.PuntosCoches);
        listaPuntosOrdenada.Sort();
        listaPuntosOrdenada.Reverse();

        // Guardamos los puntos ordenados del jugador 1 al 4
        List<int> listaPuntosDesordenada = gm.PuntosCoches;

        // Mostramos los resultados
        int puesto = 0;
        int numJugador = 0;
        for (int i = 0; i < listaPuntosOrdenada.Count; i++)
        {
            for (int j = 0; j < listaPuntosDesordenada.Count; j++)
            {
                if (listaPuntosDesordenada[j] == listaPuntosOrdenada[i])
                {
                    puesto = i + 1;
                    numJugador = j + 1;
                    resultadosTexts.transform.GetChild(puesto).GetComponent<Text>().text = 
                        puesto.ToString() + "º J" + numJugador.ToString() + " " + listaPuntosOrdenada[i].ToString() + " P";
                }
            }
        }

        // Activamos el panel de los resultados
        resultadosPanel.SetActive(true);
    }

    public void Continuar()
    {
        // Desactivamos el panel de los resultados y mostramos la selección de opciones
        resultadosPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("TFG/Scenes/Tablero");
        Debug.Log("Cargando menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitando juego...");
        Application.Quit();
    }
}
