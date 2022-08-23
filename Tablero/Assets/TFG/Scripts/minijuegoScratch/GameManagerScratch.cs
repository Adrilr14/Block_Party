using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManagerScratch : MonoBehaviour
{
    // Scripts
    public CocheScratch cocheScratch;
    public GameManager gm;

    // Escenas
    public GameObject tableroScene;
    public GameObject juegoScratchScene;

    // Variables para el puzzle
    public GameObject buttonPrefabOn;
    public GameObject buttonPrefabAvanzar;
    public GameObject buttonPrefabLeft;
    public GameObject buttonPrefabRight;
    public GameObject buttonPrefabForInit;
    public GameObject buttonPrefabForEnd;
    public GameObject buttonPrefabOff;
    public GameObject scrollContenido;
    public Scrollbar scrollbarHorizontal;
    public InputField resetInputField;

    // Variables de clase
    private int puntos;
    private int nivel;
    private int ruedas;
    [System.NonSerialized] public List<GameObject> ruedasCogidasList; // Lista para guardar las ruedas cogidas y volver a activarlas al final del nivel

    // Variables de mapas
    private GameObject mapaActual;
    private List<List<GameObject>> mapas;
    public List<GameObject> mapas1;
    public List<GameObject> mapas2;
    public List<GameObject> mapas3;
    public List<GameObject> mapas4;
    public List<GameObject> mapas5;
    public List<GameObject> mapas6;

    // Variables reloj
    private float timeRemaining;
    [System.NonSerialized] public bool timerIsRunning;
    public Text timeText;

    // Ventanas
    public GameObject mensajeErrorPanel;
    public GameObject mensajeFinJuegoPanel;
    public GameObject instruccionesPanel;

    public int Nivel { get => nivel; set => nivel = value; }
    public int Ruedas { get => ruedas; set => ruedas = value; }
    public float TimeRemaining { get => timeRemaining; set => timeRemaining = value; }
    //public bool TimerIsRunning { get => timerIsRunning; set => timerIsRunning = value; }

    // Start is called before the first frame update
    void Awake()
    {
        // Inicializamos algunas variables
        puntos = 0;
        nivel = 0;
        ruedas = 0;
        ruedasCogidasList = new List<GameObject>();
        timeRemaining = 60;
        timerIsRunning = false;

        // Añadimos las listas de mapas en la lista de listas de mapasç
        mapas = new List<List<GameObject>>();
        mapas.Add(mapas1);
        mapas.Add(mapas2);
        mapas.Add(mapas3);
        mapas.Add(mapas4);
        mapas.Add(mapas5);
        mapas.Add(mapas6);
    }

    // Update is called once per frame
    void Update()
    {
        // Controlar temporizador
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
            displayTime(timeRemaining);
        }
    }

    // Se mostrará el canvas con las instrucciones para jugar
    public void startGame(int numCasilla)
    {
        asignarNivel(numCasilla);
        generarMapa();
        instruccionesPanel.SetActive(true);
    }

    public void asignarNivel(int numCasilla)
    {
        if (numCasilla < 7) nivel = 0;
        else if (numCasilla < 13) nivel = 1;
        else if (numCasilla < 19) nivel = 2;
        else if (numCasilla < 25) nivel = 3;
        else if (numCasilla < 31) nivel = 4;
        else if (numCasilla < 37) nivel = 5;
    }

    public void generarMapa()
    {
        int rand = Random.Range(0, mapas[nivel].Count);  // Sacar numero random de la cantidad de mapas que hay
        //nivel = 0;
        //rand = 0;
        mapaActual = mapas[nivel][rand];
        mapaActual.SetActive(true);
        asignarTiempo();
    }

    private void asignarTiempo()
    {
        //TODO: comprobar si este tiempo está bien para la dificultad de los niveles
        switch (nivel)
        {
            case 0:
                timeRemaining = 60;
                break;
            case 1:
                timeRemaining = 70;
                break;
            case 2:
                timeRemaining = 90;
                break;
            case 3:
                timeRemaining = 100;
                break;
            case 4:
                timeRemaining = 120;
                break;
            case 5:
                timeRemaining = 140;
                break;
            default:
                timeRemaining = 100;
                break;
        }
    }

    // Creamos un nuevo botón según el tag de la pieza
    public void addPiezaButton(GameObject itemDropping)
    {
        GameObject auxBtn;
        string auxText = "";
        int textToNumber = 0;
        int btnAdded = 0; // 0 = Crear botón; 1 = Texto vacío; 2 = Texto igual a "0" o "00"
        bool btnForInitAdded = false;

        switch (itemDropping.tag)
        {
            case "PiezaOn":
                auxBtn = buttonPrefabOn;
                cocheScratch.addMovement(1, 0);
                break;

            case "PiezaAvanzar":
                auxBtn = buttonPrefabAvanzar;
                cocheScratch.addMovement(2, 0);
                break;

            case "PiezaLeft":
                auxBtn = buttonPrefabLeft;
                cocheScratch.addMovement(3, 0);
                break;

            case "PiezaRight":
                auxBtn = buttonPrefabRight;
                cocheScratch.addMovement(4, 0);
                break;

            case "PiezaForInit":
                auxBtn = buttonPrefabForInit;
                auxText = itemDropping.transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text; // (Es 2 porque el input crea un objeto en ejecución)
                if (auxText != "")
                {
                    textToNumber = int.Parse(auxText);
                    if (textToNumber <= 0)
                        btnAdded = 2;
                    else
                    {
                        btnForInitAdded = true;
                        cocheScratch.addMovement(5, textToNumber);
                    }
                }   
                else
                    btnAdded = 1;
                break;

            case "PiezaForEnd":
                auxBtn = buttonPrefabForEnd;
                cocheScratch.addMovement(6, 0);
                break;

            case "PiezaOff":
                auxBtn = buttonPrefabOff;
                cocheScratch.addMovement(7, 0);
                break;

            default:
                auxBtn = buttonPrefabOn;
                cocheScratch.addMovement(1, 0);
                break;
        }
        if (btnAdded == 0)
        {
            GameObject newButton = Instantiate(auxBtn, scrollContenido.transform) as GameObject;
            newButton.name = scrollContenido.transform.childCount.ToString();
            if (btnForInitAdded)
            {
                clearInputField(resetInputField);
                newButton.transform.GetChild(0).GetComponent<Text>().text = textToNumber.ToString();
            }
            newButton.GetComponent<Button>().onClick.AddListener(() => deleteButton(newButton));
            StartCoroutine(resetScrollbar());
        }
        else if (btnAdded == 1)
        {
            mensajeErrorCampoVacio();
        }
        else
        {
            mensajeErrorValor0();
        }
    }

    public IEnumerator resetScrollbar()
    {
        yield return new WaitForSeconds(0.1f);
        scrollbarHorizontal.value = 1;
    }

    // Función para limpiar un InputField
    public void clearInputField(InputField inputField)
    {
        inputField.text = "";
    }

    private void mensajeErrorCampoVacio()
    {
        mensajeErrorPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Falta el valor de repetición";
        mensajeErrorPanel.SetActive(true);
    }

    private void mensajeErrorValor0()
    {
        clearInputField(resetInputField);
        mensajeErrorPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Añade un valor mayor que 0";
        mensajeErrorPanel.SetActive(true);
    }

    public void cerrarIntrucciones()
    {
        timerIsRunning = true;
        instruccionesPanel.SetActive(false);
    }

    private void deleteButton(GameObject button)
    {
        string nameButton = button.name;
        int num = int.Parse(nameButton) - 1;

        for (int i = num; i < scrollContenido.transform.childCount; i++)
        {
            scrollContenido.transform.GetChild(i).name = i.ToString();
        }
        Destroy(button);
        cocheScratch.deleteMovement(num);
    }

    public void deleteAllButtons()
    {
        foreach (Transform child in scrollContenido.transform)
        {
            Destroy(child.gameObject);
        }
        cocheScratch.deleteCode();
    }

    private void displayTime(float timeToDisplay)
    {
        int seconds = Mathf.FloorToInt(timeToDisplay);
        timeText.text = seconds.ToString();
    }

    public void finJuego()
    {
        if (nivel == 0 || nivel == 1)
        {
            finJuegoPasado();
        }
        else if (nivel == 2 || nivel == 3)
        {
            if (ruedas == 1)
                finJuegoPasado();
            else
                finJuegoSinRuedas();
        }
        else if (nivel == 4)
        {
            if (ruedas == 2)
                finJuegoPasado();
            else if (ruedas != 0)
                finJuegoConAlgunaRueda();
            else
                finJuegoSinRuedas();
        }
        else if (nivel == 5)
        {
            if (ruedas == 3)
                finJuegoPasado();
            else if (ruedas != 0)
                finJuegoConAlgunaRueda();
            else
                finJuegoSinRuedas();
        }
    }

    public void finJuegoPasado()
    {
        timerIsRunning = false;
        puntos = 50 + ((int)timeRemaining);
        mensajeFinJuegoPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text =
            "¡Enhorabuena! Has superado el nivel y has ganado " + puntos.ToString() + " puntos.";
        mensajeFinJuegoPanel.SetActive(true);
    }

    public void finJuegoSinRuedas()
    {
        timerIsRunning = false;
        puntos = 10 + (((int)timeRemaining)/3);
        mensajeFinJuegoPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text =
            "Has superado el nivel pero no has recogido ninguna rueda. Solo ganas " + puntos.ToString() + " puntos.";
        mensajeFinJuegoPanel.SetActive(true);
    }

    public void finJuegoConAlgunaRueda()
    {
        timerIsRunning = false;
        if (ruedas == 1) puntos = 20 + (((int)timeRemaining)/2);
        if (ruedas == 2) puntos = 30 + (((int)timeRemaining)/2);
        mensajeFinJuegoPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text =
            "Has superado el nivel pero no has recogido todas las ruedas. Ganas " + puntos.ToString() + " puntos.";
        mensajeFinJuegoPanel.SetActive(true);
    }

    public void finJuegoSinPasar()
    {
        mensajeFinJuegoPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text =
            "Vaya... no has superado el nivel y no has ganado puntos.";
        mensajeFinJuegoPanel.SetActive(true);
    }

    public void partidaTerminada()
    {
        mapaActual.SetActive(false);
        mensajeFinJuegoPanel.SetActive(false);
        //cocheScratch.resetCoche();
        cocheScratch.startAgain(); // Ya se hace el reset del coche en esta fundión
        gm.desactivarCamaras();
        tableroScene.SetActive(true);
        juegoScratchScene.SetActive(false);
        gm.siguienteJugador(puntos, false);
        //enemy.gameObject.transform.position = enemy.PositionInit;
        //enemy.gameObject.transform.rotation = enemy.RotationInit;
        timeRemaining = 60;
        timeText.text = ""; // Quitamos los segundos que faltaban
        puntos = 0;
        nivel = 0;
        ruedas = 0;
        for (int i = 0; i < ruedasCogidasList.Count; i++) // Activamos las ruedas previamente desactivadas por cogerlas
            ruedasCogidasList[i].SetActive(true);
    }

}
