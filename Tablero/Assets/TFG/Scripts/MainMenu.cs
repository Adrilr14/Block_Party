using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject mainMenu;
    public GameObject controlsMenu;
    public GameObject playersMenu;
    public GameObject difficultyMenu;
    public GameObject resumenMenu;

    private int numPlayers; // 1, 2, 3, o 4
    private string level; //facil, dificil
    public TextMeshProUGUI casPlayers;
    public TextMeshProUGUI casNivel;

    //scripts
    public GameManager gm;

    public string Level { get => level; set => level = value; }

    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        mainMenuCanvas.gameObject.SetActive(false);
        // Cambiamos el estado del juego
        gm.cambioEstadoJuego = true;
        if(numPlayers > 1) gm.estadoJuego = 0;
        else gm.estadoJuego = -2;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectJugadores(int numJugadores)
    {
        playersMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        numPlayers = numJugadores;
        // Cambiamos el número de jugadores del juego
        gm.numJugadores = numPlayers;
    }

    public void SelectNivel(string nivel)
    {
        Level = nivel;
        Resumen();
    }

    public void Resumen()
    {
        difficultyMenu.SetActive(false);
        resumenMenu.SetActive(true);
        casPlayers.text = numPlayers.ToString();
        casNivel.text = Level;
    }
}
