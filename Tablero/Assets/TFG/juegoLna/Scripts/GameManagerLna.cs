using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerLna : MonoBehaviour
{
    public GameObject canvasStart;
    public GameObject canvasFinal;
    public Image img; //imagen fade out

    //escenas
    public GameObject tablero;
    public GameObject juegoFacil;

    public Text textoFinal;
    /*estados del cronometro*/
    private bool activo;

    //reloj
    private float timeRemaining;
    internal bool timerIsRunning;
    public Text timeText;

    GameObject mapa;

    private int puntos;
    private int nivel;
    private int rueda;

    private GameObject mapaActual;
    private int mapasPasados;

    public GameObject enemigo;
    //scripts
    public Coche coche;
    public GameManager gManager;

    public Enemigo enemy;
    //mapas
    private List<List<GameObject>> mapas;
    public List<GameObject> mapas1;
    public List<GameObject> mapas2;
    public List<GameObject> mapas3;
    public List<GameObject> mapas4;
    public List<GameObject> mapas5;
    public List<GameObject> mapas6;

    //sonido
    private AudioSource audioSource;
    public AudioClip congrats;
    public AudioClip delete;
    public AudioClip error;
    public AudioClip select;
    public AudioClip musica;

    public int Nivel { get => nivel; set => nivel = value; }
    public int Rueda { get => rueda; set => rueda = value; }
    public float TimeRemaining { get => timeRemaining; set => timeRemaining = value; }

    private void Start()
    {
        mapas = new List<List<GameObject>>();
        mapas.Add(mapas1);
        mapas.Add(mapas2);
        mapas.Add(mapas3);
        mapas.Add(mapas4);
        mapas.Add(mapas5);
        mapas.Add(mapas6);

        canvasStart.SetActive(true);

        timeRemaining = 60;
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //empezar temporizador
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

            DisplayTime(timeRemaining);
        }
    }
    //empezará a contar la cuenta atrás y cambiará de canvas
    public void startGame()
    {
        canvasStart.SetActive(false);
        timerIsRunning = true;
        activo = true;
        generarMapa();
        audioSource.PlayOneShot(musica);
    }
    public void asignarNivel(int numCasilla)
    {
        if(numCasilla < 7) Nivel = 0;
        else if(numCasilla < 13) Nivel = 1;
        else if(numCasilla < 19) Nivel = 2;
        else if(numCasilla < 25) Nivel = 3;
        else if(numCasilla < 31) Nivel = 4;
        else if (numCasilla < 37) Nivel = 5;
    }
    public void generarMapa()
    {
        int rand = Random.Range(0, mapas[Nivel].Count);  // Sacar numero random de la cantidad de mapas que hay
        //Nivel = 1;
        //rand = 0;
        mapa = mapas[Nivel][rand];
        mapa.SetActive(true);
        AsignarTiempo();
        //TODO: cambiar el tiempo a los siguientes niveles
    }

    private void AsignarTiempo()
    {
        switch (Nivel)
        {
            case 0:
                timeRemaining = 60;
                break;
            case 1:
                timeRemaining = 80;
                break;
            case 2:
                timeRemaining = 90;
                break;
            case 3:
                timeRemaining = 100;
                break;
            case 4:
                timeRemaining = 110;
                break;
            case 5:
                timeRemaining = 110;
                break;
            default:
                timeRemaining = 110;
                break;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int seconds = Mathf.FloorToInt(timeToDisplay);
        timeText.text = seconds.ToString();
    }

    /*
    public void mapaPasado()
    {
        cantMapas++;
        // fades the image out when you click
        if (mapas.Count > 0)
        {
            StartCoroutine(FadeImage());
            Debug.Log("mapas.count: " + mapas.Count);
        }
        else
        {
            finJuego();
        }

    }
    IEnumerator FadeImage()
    {
        img.gameObject.SetActive(true);

        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        if (mapas.Count > 0)
        {
            mapaActual.SetActive(false);
            int rand = Random.Range(0, mapas.Count);  // Sacar numero random de la cantidad de mapas que hay
            mapas[rand].SetActive(true);
            mapaActual = mapas[rand];
            mapas.RemoveAt(rand);
        }
        
        coche.StartAgain();
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime) //time.delatime dura de un upadte a otro
        {
            // set color with i as alpha
            img.color = new Color(0, 0, 0, i);
            yield return null;
        }
        img.gameObject.SetActive(false);
    }
    */
    public void comprobarRueda()
    {
        if(Nivel == 2 && Rueda == 1) //nivel 3 real, nivel 2 en el codigo
        {
            FinJuegoPasado();

        }else if(Nivel == 3 && Rueda == 2 )
        {
            FinJuegoPasado();
        }else if(Nivel == 3 && Rueda == 1)
        {
            FinJuegoSoloUnaRueda();
        }
        else if(Nivel == 5 && Rueda == 1)
        {
            FinJuegoPasado();
        }else
        {
            FinJuegoSinRueda();
        }
    }

    private void FinJuegoSoloUnaRueda()
    {
        timerIsRunning = false;
        textoFinal.text = "Vaya... Has alcanzado la meta con una rueda solo, te llevas 10 puntos";
        puntos = 0;
        canvasFinal.SetActive(true);
        Rueda = 0;
    }

    public void FinJuegoPasado()
    {
        timerIsRunning = false;
        audioSource.PlayOneShot(congrats);
        puntos = Mathf.FloorToInt(timeRemaining);
        textoFinal.text = "La suma de puntos que has conseguido es... \n" + puntos + " puntos";
        canvasFinal.SetActive(true);
        Rueda = 0;
    }

    public void FinJuegoSinPasar()
    {

        timerIsRunning = false;
        textoFinal.text = "Vaya... No has alcanzado la meta, te llevas 0 puntos";
        puntos = 0;
        canvasFinal.SetActive(true);
        Rueda = 0;
    }

    public void FinJuegoSinRueda()
    {
        timerIsRunning = false;
        textoFinal.text = "Vaya... Has alcanzado la meta sin la rueda, te llevas 5 puntos";
        puntos = 5;
        canvasFinal.SetActive(true);
        Rueda = 0;
    }

    //sonidos
    public void sonarDelete()
    {
        audioSource.PlayOneShot(delete);
    }
    public void sonarError()
    {
        audioSource.PlayOneShot(error);
    }
    public void sonarSelect()
    {
        audioSource.PlayOneShot(select);
    }
    //getters
    public bool getActivo()
    {
        return activo;
    }

    public void partidaTerminada()
    {
        mapa.SetActive(false);
        canvasFinal.SetActive(false);
        canvasStart.SetActive(true);
        coche.ResetCoche();
        gManager.desactivarCamaras();
        juegoFacil.SetActive(false);
        tablero.SetActive(true);
        gManager.siguienteJugador(puntos, false);
        enemy.gameObject.transform.position = enemy.PositionInit;
        enemy.gameObject.transform.rotation = enemy.RotationInit;
        timeRemaining = 60;
    }
}
