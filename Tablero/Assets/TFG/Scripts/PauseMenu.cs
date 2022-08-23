using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused;

    public GameObject pauseMenuUI;

    //scripts
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && gm.estadoJuego != -1 && gm.estadoJuego != -3 && gm.estadoJuego != 7)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if(!ActivarCamaraMapa.MapaIsActive) Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
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
