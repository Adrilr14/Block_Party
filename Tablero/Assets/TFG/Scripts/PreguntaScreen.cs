using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreguntaScreen : MonoBehaviour
{
    // Objects
    public GameObject preguntasScreenPanel;
    public GameObject preguntaPanel;
    public GameObject puntosScreenPanel;
    public Button respuestaButton1;
    public Button respuestaButton2;
    public Button respuestaButton3;
    public Button respuestaButton4;
    public TextAsset assetPreguntas;
    public TextAsset assetRespuestas;
    public Sprite selectedGreenSprite;
    public Sprite selectedRedSprite;
    public Sprite disabledSprite;
    public Sprite disabledNormalSprite;

    // Variables
    private List<string> preguntas;
    private string[] auxPreguntas;
    private List<List<string>> respuestas;
    private string[] auxRespuestas;
    private List<string> auxListaRespuestas;
    private int preguntaSeleccionada;
    private float tiempo;
    private bool buttonClicked;
    SpriteState spriteStateAux;
    private int puntos;

    // Scripts
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializamos algunas variables
        tiempo = 0f;
        buttonClicked = false;
        spriteStateAux = new SpriteState();
        puntos = 0;

        // Inicializamos lasa listas
        preguntas = new List<string>();
        respuestas = new List<List<string>>();
        auxListaRespuestas = new List<string>();

        // Creamos un array de char para añadir los delimitadores que usaremos para el texto
        char[] delims = new[] { '\r', '\n' };

        // Inicializamos el array auxiliar de preguntas con el TextAsset de las preguntas
        auxPreguntas = assetPreguntas.text.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);

        // Rellenamos la lista de preguntas con el array auxiliar de preguntas
        for (int i = 0; i < auxPreguntas.Length; i++)
        {
            preguntas.Add(auxPreguntas[i]);
        }

        // Inicializamos el array auxiliar de respuestas con el TextAsset de las respuestas
        auxRespuestas = assetRespuestas.text.Split(delims);

        // Rellenamos la lista de listas de respuestas con el array auxiliar de respuestas (dos saltos de línea cambiamos de fila)
        for (int i = 0; i < auxRespuestas.Length; i++)
        {
            // Si es un espacio en blanco cmaiamos avanzamos, si son 2 agregamos la auxListaRespuestas a respuestas y reiniciamos auxListaRespuestas
            if (auxRespuestas[i] == "")
            {
                i++;
                if (auxRespuestas[i] == "")
                {
                    i += 2;
                    respuestas.Add(new List<string>(auxListaRespuestas));
                    auxListaRespuestas.Clear();
                }
            }
            // Vamos agregando a la lista las distintas respuestas
            auxListaRespuestas.Add(auxRespuestas[i]);

            // Si hemos llegado al final agrgamos la lista auxListaRespuestas a la lista de respuestas y reiniciamos la lista auxListaRespuestas
            if (i >= auxRespuestas.Length - 1)
            {
                respuestas.Add(new List<string>(auxListaRespuestas));
                auxListaRespuestas.Clear();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (buttonClicked && tiempo <= Time.time)
        {
            // Desactivamos el panel de preguntas
            preguntasScreenPanel.SetActive(false);

            // Activamos el panel de puntos
            puntosScreenPanel.SetActive(true);

            // Ponemos a false el buttonClicked
            buttonClicked = false;
        }
    }

    public void Respuesta1()
    {
        if (!buttonClicked)
        {
            // Ponemos a true buttonClicked y el resto de botones con el atributo "Interactable" a false para que no se puedan presionar
            buttonClicked = true;
            respuestaButton2.interactable = false;
            respuestaButton3.interactable = false;
            respuestaButton4.interactable = false;

            // Guardamos el tiempo actual + 1 segundo
            tiempo = Time.time + 1f;

            // Si la respuesta coincide con la primera respuesta del array de respuestas será la correcta y ponemos el selectedGreenSprite
            if (respuestaButton1.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == respuestas[preguntaSeleccionada][0])
            {
                spriteStateAux = respuestaButton1.spriteState;
                spriteStateAux.selectedSprite = selectedGreenSprite;
                respuestaButton1.spriteState = spriteStateAux;
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "¡Has ganado 100 puntos!";
                puntos = 100;
            }
            else
            {
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Vaya... No has ganado puntos";
                puntos = 0;
            }
        }
    }
    public void Respuesta2()
    {
        if (!buttonClicked)
        {
            // Ponemos a true buttonClicked y el resto de botones con el atributo "Interactable" a false para que no se puedan presionar
            buttonClicked = true;
            respuestaButton1.interactable = false;
            respuestaButton3.interactable = false;
            respuestaButton4.interactable = false;

            // Guardamos el tiempo actual + 1 segundo
            tiempo = Time.time + 1f;

            // Si la respuesta coincide con la primera respuesta del array de respuestas será la correcta y ponemos el selectedGreenSprite
            if (respuestaButton2.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == respuestas[preguntaSeleccionada][0])
            {
                spriteStateAux = respuestaButton2.spriteState;
                spriteStateAux.selectedSprite = selectedGreenSprite;
                respuestaButton2.spriteState = spriteStateAux;
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "¡Has ganado 100 puntos!";
                puntos = 100;
            }
            else
            {
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Vaya... No has ganado puntos";
                puntos = 0;
            }
        }
    }
    public void Respuesta3()
    {
        if (!buttonClicked)
        {
            // Ponemos a true buttonClicked y el resto de botones con el atributo "Interactable" a false para que no se puedan presionar
            buttonClicked = true;
            respuestaButton1.interactable = false;
            respuestaButton2.interactable = false;
            respuestaButton4.interactable = false;

            // Guardamos el tiempo actual + 1 segundo
            tiempo = Time.time + 1f;

            // Si la respuesta coincide con la primera respuesta del array de respuestas será la correcta y ponemos el selectedGreenSprite
            if (respuestaButton3.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == respuestas[preguntaSeleccionada][0])
            {
                spriteStateAux = respuestaButton3.spriteState;
                spriteStateAux.selectedSprite = selectedGreenSprite;
                respuestaButton3.spriteState = spriteStateAux;
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "¡Has ganado 100 puntos!";
                puntos = 100;
            }
            else
            {
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Vaya... No has ganado puntos";
                puntos = 0;
            }
        }
    }
    public void Respuesta4()
    {
        if (!buttonClicked)
        {
            // Ponemos a true buttonClicked y el resto de botones con el atributo "Interactable" a false para que no se puedan presionar
            buttonClicked = true;
            respuestaButton1.interactable = false;
            respuestaButton2.interactable = false;
            respuestaButton3.interactable = false;

            // Guardamos el tiempo actual + 1 segundo
            tiempo = Time.time + 1f;

            // Si la respuesta coincide con la primera respuesta del array de respuestas será la correcta y ponemos el selectedGreenSprite
            if (respuestaButton4.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == respuestas[preguntaSeleccionada][0])
            {
                spriteStateAux = respuestaButton1.spriteState;
                spriteStateAux.selectedSprite = selectedGreenSprite;
                respuestaButton4.spriteState = spriteStateAux;
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "¡Has ganado 100 puntos!";
                puntos = 100;
            }
            else
            {
                puntosScreenPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Vaya... No has ganado puntos";
                puntos = 0;
            }
        }
    }

    public void Continuar()
    {
        // Desactivamos el panel de puntos
        puntosScreenPanel.SetActive(false);

        // Cambiamos el estado del juego
        gm.estadoJuego = 1;
        gm.desactivarCamaras();
        gm.siguienteJugador(puntos, false);
    }

    public void mostrarPreguntaYRespuesta(int numCasilla)
    {
        inicializarRespuestas();

        // Seleccionamos una pregunta random
        preguntasSinDificultad();
        //preguntasConDificultad(numCasilla);

        // Cambiamos el texto de la pregunta
        preguntaPanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = preguntas[preguntaSeleccionada];

        mostrarRespuestasAleatorias();

        respuestaIsInteractabe();

        // Activamos el panel de preguntas
        preguntasScreenPanel.SetActive(true);
    }

    // Se llama a esta función si todas las preguntas son las mismas sin tener en cuenta la dificultad
    private void preguntasSinDificultad()
    {
        // Hacemos un random para cambiar la pregunta seleccionada
        preguntaSeleccionada = Random.Range(0, preguntas.Count);
    }

    // Se llama a esta función si hay distinción de dificultad entre las preguntas
    private void preguntasConDificultad(int numCasilla)
    {
        // Guardamos el número máximo para cada dificultad
        // TODO: Cambiar preguntas.Count por el número de preguntas de cada dificultad
        int preguntasMuyFacil = preguntas.Count;
        int preguntasFacil = preguntas.Count;
        int preguntasNormal = preguntas.Count;
        int preguntasDificil = preguntas.Count;
        int preguntasMuyDificil = preguntas.Count;
        int preguntasExtremo = preguntas.Count;

        // TODO: Cambiar los 0 por la dificultad anterior +1 cuando tenga las preguntas
        // Seleccionamos una pregunta random dependiendo de la zona de dificultad en la que se encuentra la casilla
        if (numCasilla < 7) preguntaSeleccionada = Random.Range(0, preguntasMuyFacil);
        else if (numCasilla < 13) preguntaSeleccionada = Random.Range(0, preguntasFacil);
        else if (numCasilla < 19) preguntaSeleccionada = Random.Range(0, preguntasNormal);
        else if (numCasilla < 25) preguntaSeleccionada = Random.Range(0, preguntasDificil);
        else if (numCasilla < 31) preguntaSeleccionada = Random.Range(0, preguntasMuyDificil);
        else if (numCasilla < 37) preguntaSeleccionada = Random.Range(0, preguntasExtremo);
    }

    private void inicializarRespuestas()
    {
        // Vaciamos las respuestas
        respuestaButton1.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
        respuestaButton2.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
        respuestaButton3.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
        respuestaButton4.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";

        // Ponemos a true el atributo "Interactable" de los botones
        respuestaButton1.interactable = true;
        respuestaButton2.interactable = true;
        respuestaButton3.interactable = true;
        respuestaButton4.interactable = true;

        // Ponemos con el sprite disabled none las respuestas
        spriteStateAux = respuestaButton1.spriteState;
        spriteStateAux.disabledSprite = disabledNormalSprite;
        respuestaButton1.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton2.spriteState;
        spriteStateAux.disabledSprite = disabledNormalSprite;
        respuestaButton2.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton3.spriteState;
        spriteStateAux.disabledSprite = disabledNormalSprite;
        respuestaButton3.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton4.spriteState;
        spriteStateAux.disabledSprite = disabledNormalSprite;
        respuestaButton4.spriteState = spriteStateAux;

        // Ponemos en incorrectas todas las respuestas
        spriteStateAux = respuestaButton1.spriteState;
        spriteStateAux.selectedSprite = selectedRedSprite;
        respuestaButton1.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton2.spriteState;
        spriteStateAux.selectedSprite = selectedRedSprite;
        respuestaButton2.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton3.spriteState;
        spriteStateAux.selectedSprite = selectedRedSprite;
        respuestaButton3.spriteState = spriteStateAux;

        spriteStateAux = respuestaButton4.spriteState;
        spriteStateAux.selectedSprite = selectedRedSprite;
        respuestaButton4.spriteState = spriteStateAux;
    }

    private void respuestaIsInteractabe()
    {
        // Component Interactable a false de los botones que tienen de texto "", sino a true
        spriteStateAux = respuestaButton3.spriteState;
        if (respuestaButton3.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == "")
        {
            respuestaButton3.interactable = false;
            spriteStateAux.disabledSprite = disabledSprite;
            respuestaButton3.spriteState = spriteStateAux;
        }
        else
            respuestaButton3.interactable = true;

        spriteStateAux = respuestaButton4.spriteState;
        if (respuestaButton4.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text == "")
        {
            respuestaButton4.interactable = false;
            spriteStateAux.disabledSprite = disabledSprite;
            respuestaButton4.spriteState = spriteStateAux;
        }
        else
            respuestaButton4.interactable = true;
    }

    private void mostrarRespuestasAleatorias()
    {
        // Cambiamos el texto de las respuestas
        int[] randomRespuestas = new int[respuestas[preguntaSeleccionada].Count];
        for (int i = 0; i < randomRespuestas.Length; i++)
        {
            randomRespuestas[i] = i;
        }

        randomRespuestas = Util.shuffleIntArray(randomRespuestas);
        for (int i = 0; i < respuestas[preguntaSeleccionada].Count; i++)
        {
            switch (i)
            {
                case 0:
                    respuestaButton1.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = respuestas[preguntaSeleccionada][randomRespuestas[i]];
                    break;
                case 1:
                    respuestaButton2.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = respuestas[preguntaSeleccionada][randomRespuestas[i]];
                    break;
                case 2:
                    respuestaButton3.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = respuestas[preguntaSeleccionada][randomRespuestas[i]];
                    break;
                case 3:
                    respuestaButton4.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = respuestas[preguntaSeleccionada][randomRespuestas[i]];
                    break;
                default:
                    break;
            }
        }
    }

}
