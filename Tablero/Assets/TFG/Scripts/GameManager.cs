using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Scripts
    public Movement mv;
    public SceneChange sceneChange;
    public MainMenu mainMenu;
    public GameManagerLna gManagerFacil;
    public GameManagerScratch gManagerScratch;
    public PreguntaScreen preguntaCanvas;

    // Variables dados
    public GameObject[] dados; // Objetos
    private GameObject[] dadosJugar; // Objetos ségun el numero de jugadores
    private Animator[] dadosAnim;
    private Quaternion[] rotDados;
    private int[] dadosStates; // Array de estados de los dados --> 0 = "dado_parado"; 1 = "dado_girando"
    private GameObject dadoActual; //dado del coche que esta jugando
    private int numDado; //numero que se ha sacado el jugador que toca

    // Variables coches
    public GameObject[] coches; // Objetos
    private GameObject[] cochesJugar; // Objetos ségun el numero de jugadores
    private Animator[] cochesAnim;
    private int[] cochesStates; // Array de estados de los coches --> 0 = "coche_parado"; 1 = "coche_saltando"
    private GameObject[] cochesOrdenados; //orden de los coches segun los dados
    private int numCoche; //el int que se le pasa a saltarCoche(), posicion del numJugadorActual en el array de coches
    public int[] posCoches; //array de ints que contienen la posicion de cada coche
    private List<int> puntosCoches; //array que contiene los puntos de cada coche en orden de jugadores (de 1 a 4)
    public List<int> PuntosCoches { get => puntosCoches; set => puntosCoches = value; } // setter y getter para la variable puntos coches

    // Variables juego
    public int numJugadores; // Variable numero de jugadores
    private int[] numeroSacadoJugador; // Numero del dado para el orden o para avanzar en el tablero
    private List<int> numeroSacadoJugadorOrdenado; // List para guardar los numeros sacados por los jugadores y ordenarlos
    private List<int> ordenTurnosCoches; // Orden los jugadores para avanzar
    private int[] ordenTurnosCochesAux; // Array para guardar antes los números y después añadirlos en el de orden sin chafar el orden que ya habia (cuando se repiten numeros)
    private List<int> repetirTirada; // List para saber que coches tienen que repetir una tirada por la repetición de un número
    private bool repetirTiradaInicialEjecutada; //  Variable para saber si hemos ejecutado la función RepetirTiradaInicial()
    private bool calculoTirada; // Variable para saber si comprobamos que los dados no se mueven
    private int estadoTirada; // Estados tirada --> 0 = nada; 1 = calcular turnos; 2 = repeticion tirada; 3 y 4 = acabada (comprobar que pase un tiempo)
    private bool cambiarEstadoTirada; // Varible para saber si cambiamos el estado de la tirada en ciertas circunstancias
    private float tiempo; // Variable para conocer el tiempo que ha pasado desde cierto momento
    private List<int> numJugadoresEliminados; // Variable para guardar el número de jugadores

    // Varible estado juego
    public int estadoJuego; // Estados juego --> 0 = Calculo de turnos; 1 = Movimiento tablero; -1 = menu principal; -2 = estado especial solo 1 jugador; 
                            //                   2 = Juego fácil; 3 = Juego difícil; 4 = movimiento después de lanzar el dado; 5 = cámara superior;
                            //                  -3 = Pantalla de inicio; 6 = preguntas; 7 = final de juego;
    // 2 = estado juego elena
    public bool cambioEstadoJuego;
    public bool stop;
    private int numJugadorActual;
    private int[] pos;

    //variables para MOVER el coche
    public GameObject posInicial;

    //Escenas
    public GameObject escenaJF;
    public GameObject escenaJD;
    public GameObject escenaTablero;
    public GameObject ruleta;

    //Camaras
    public Camera camaraPrincipal;
    public Camera camaraMapa;
    public Camera camaraJuegoFacil;

    //Canvas de los puntos
    public GameObject canvasPuntos;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f; // Parar la ejecución del juego

        //Ponemos el estado del juego a -1 que es la pantalla de inicio
        estadoJuego = -3;

        // Activamos la cámara principal y desactivamos el resto
        camaraMapa.enabled = false;
        camaraPrincipal.enabled = true;
    }

    //bool empezar = false;
    //int f = 0;
    // Update is called once per frame
    void Update()
    {
        comprobarEstadoJuego();

        /*if (canvasPuntos.activeSelf)
        {
            StartCoroutine(imprimirPuntos());
        }*/
    }

    private void comprobarEstadoJuego()
    {
        if (estadoJuego == 0)
        {
            if (cambioEstadoJuego)
            {
                inicioEstado0();
                cambioEstadoJuego = false;
            }
            estado0();
        }

        else if (estadoJuego == 1)
        {
            if (cambioEstadoJuego)
            {
                inicioEstado1();
                cambioEstadoJuego = false;
            }
            estado1(); //coche tirando su dado
        }

        else if (estadoJuego == -2)
        {
            if (cambioEstadoJuego)
            {
                inicioEstado0(); // Iniciamos igual las variables como si fuera el estado 0
                cambioEstadoJuego = false;
            }
            estado_2();
        }
    }

    private void inicioEstado1()
    {
        numJugadorActual = 0; // Reiniciamos el juagdor actual a 0 para coger el primer coche del vector ordenCoches
        desactivarDados(); // Desactivamos los dados
        tiradaCoche(cochesOrdenados[0]); // Activamos el coche que va a tirar primero
        canvasPuntos.SetActive(true);//Activamos el HUD de los puntos
        for(int x = 0; x < numJugadores; x++) // Activamos los marcadores de puntos dependiendo el número de jugadores
        {
            canvasPuntos.transform.GetChild(x).gameObject.SetActive(true);
        }
        //canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetComponent<Image>().color = new Color(0, 0, 0, 100f / 255f); // Activamos la selección del jugador actual (primero ordenTurnosCoches)
        // Cambiamos el color de los textos a negro del jugador activo
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(0).GetComponent<UnityEngine.UI.Text>().color = Color.black;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(2).GetComponent<UnityEngine.UI.Text>().color = Color.black;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(3).GetComponent<UnityEngine.UI.Text>().color = Color.black;
    }

    private void estado_2()
    {
        cochesOrdenados[0] = cochesJugar[0]; // Ponemos al único jugador en la única posición del vector
        ordenTurnosCoches[0] = 0; // Ponemos en el orden de turnos que el único jugador que hay sea el primero
        estadoJuego = 1; // Cambiamos el estado del juego
        cambioEstadoJuego = true;
    }

    private void inicioEstado0()
    {
        Time.timeScale = 1f; // Reanudamos la ejecución del juego

        // Inicializamos las variables de los dados
        dadosAnim = new Animator[numJugadores];
        dadosJugar = new GameObject[numJugadores];
        dadosStates = new int[numJugadores];

        // Inicializamos las variables de los coches
        cochesAnim = new Animator[numJugadores];
        cochesStates = new int[numJugadores];
        cochesJugar = new GameObject[numJugadores];
        posCoches = new int[numJugadores];
        puntosCoches = new List<int>(numJugadores);

        // Inicializamos las variables del juego
        numJugadorActual = 0;
        numeroSacadoJugador = new int[numJugadores];
        numeroSacadoJugadorOrdenado = new List<int>(numJugadores);
        ordenTurnosCoches = new List<int>(numJugadores);
        ordenTurnosCochesAux = new int[numJugadores];
        repetirTirada = new List<int>(numJugadores);
        repetirTiradaInicialEjecutada = false;
        calculoTirada = true;
        estadoTirada = 0;
        cambiarEstadoTirada = false;
        tiempo = 0f;
        numJugadoresEliminados = new List<int>();

        pos = new int[numJugadores];

        //inicializamos array con los coches ordenados
        cochesOrdenados = new GameObject[numJugadores]; // coches ordenados según los dados

        // Inicializamos el array de rotaciones de los dados para las 6 caras
        rotDados = new Quaternion[6]
            {Quaternion.Euler(270,0,180),           // Cara 1
            Quaternion.Euler(180,90,90),           // Cara 2
            Quaternion.Euler(0,0,180),     // Cara 3
            Quaternion.Euler(0,0,0),        // Cara 4
            Quaternion.Euler(0,90,90),          // Cara 5
            Quaternion.Euler(90,90,90)};         // Cara 6

        //Anyadimos las rotaciones de cada posicion del tablero

        // Anyadimos los objetos a sus respectivos arrays y ponemos estado a la animación
        for (int i = 0; i < numJugadores; i++)
        {
            // Activamos los coches (y dados) según el número de jugadores
            coches[i].SetActive(true);

            // Dados
            dadosJugar[i] = dados[i];
            dadosAnim[i] = dadosJugar[i].transform.GetChild(0).gameObject.GetComponent<Animator>();
            dadosAnim[i].SetBool("movimiento",true);
            dadosStates[i] = 1;

            // Coches 
            cochesJugar[i] = coches[i];
            cochesAnim[i] = cochesJugar[i].transform.GetChild(0).gameObject.GetComponent<Animator>();
            cochesStates[i] = 0;

            //Juego
            numeroSacadoJugador[i] = -1;
            numeroSacadoJugadorOrdenado.Add(-1);
            ordenTurnosCoches.Add(-1);
            ordenTurnosCochesAux[i] = -1;
            puntosCoches.Add(0);
        }
    }

    private void estado1()
    {
        canvasPuntos.SetActive(true); // Activamos el canva de puntos
        StartCoroutine(imprimirPuntos()); // Imprimimos los puntos de los jugadores

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > tiempo && dadosStates[ordenTurnosCoches[numJugadorActual]] != 0)
        {
            SaltarCoche(ordenTurnosCoches[numJugadorActual]);
            estadoJuego = 4; // Cambiamos el estado de juego a movimiento
        }

    }

    private IEnumerator llamarMovCoche()
    {
        yield return new WaitForSeconds(1.5f);
        mv.moverCoche(numDado - 1, cochesOrdenados[numJugadorActual], posCoches[numJugadorActual]);
        asignarCasilla();
        canvasPuntos.SetActive(true);
    }

    private IEnumerator imprimirPuntos()
    {
        yield return new WaitForSeconds(0.2f);
        for(int x = 0; x < numJugadores; x++)
        {
            canvasPuntos.transform.GetChild(x).GetChild(2).GetComponent<UnityEngine.UI.Text>().text = puntosCoches[x].ToString();
        }
    }

    // La usamos para calcular los puntos sin esperar nada de tiempo
    private void imprimirPuntosFinal()
    {
        for (int x = 0; x < numJugadores; x++)
        {
            canvasPuntos.transform.GetChild(x).GetChild(2).GetComponent<UnityEngine.UI.Text>().text = puntosCoches[x].ToString();
        }
    }

    private void estado0()
    {
        // Comprobamos que se pulsa el espacio (tecla) (para la primera tirada y poder calcular el orden)
        if (numJugadorActual < numJugadores && Input.GetKeyDown(KeyCode.Space))
        {
            if (cochesStates[numJugadorActual] == 0)
            {
                SaltarCoche(numJugadorActual); // Si el estado del coche es igual a parado y pulsamos espacio empieza la animación
                numJugadorActual++; // Aumentamos el número del jugador actual
            }
        }

        comprobarTurnos();
        activarDadosRepetidos();
        calcularDadosTirados();
        mostrarTurnoJugadorEstado0();

        /*Debug.Log(cochesAnim[0].GetCurrentAnimatorStateInfo(0).normalizedTime);
        Debug.Log(cochesAnim[0].GetCurrentAnimatorClipInfo(0)[0].clip.frameRate * cochesAnim[0].GetCurrentAnimatorClipInfo(0)[0].clip.length);

        int dominantFrame = 0;

        if (cochesStates[0] == 1)
        {
            float nTime = cochesAnim[0].GetCurrentAnimatorStateInfo(0).normalizedTime;
            dominantFrame = Mathf.RoundToInt((nTime * (cochesAnimSaltoNumFrames[0] - 1)) % (cochesAnimSaltoNumFrames[0] - 1));
            Debug.Log("Number of frame: " + dominantFrame);
        }

        if (dominantFrame == cochesAnimSaltoNumFrames[0] - 1 && cochesStates[0] == 1)
        {
            cochesAnim[0].SetBool("movimiento",false);
            cochesStates[0] = 0;
        }*/
    }

    //desactivamos todos los dados después de tener el orden de salida
    private void desactivarDados()
    {
        foreach (var dado in dadosJugar)
        {
            dado.SetActive(false);
        }
    }

    private void calcularDadosTirados()
    {
        // Calculamos los turnos si han tirado todos el dado
        if (estadoTirada == 1)
        {
            if (numJugadores == 1)
            {
                ordenTurnosCoches[0] = 0; // Si solo hay un jugador se pone el primero a ese jugador
            }
            else
            {
                for (int x = 0; x < numJugadores - 1; x++)
                {
                    if (numeroSacadoJugador[x] != -1)
                    {
                        if (x + 1 < numJugadores && numeroSacadoJugador[x] == numeroSacadoJugador[x + 1])
                        {
                            if (!repetirTirada.Contains(x)) repetirTirada.Add(x);
                            if (!repetirTirada.Contains(x + 1)) repetirTirada.Add(x + 1);
                        }
                        if (x + 2 < numJugadores && numeroSacadoJugador[x] == numeroSacadoJugador[x + 2])
                        {
                            if (!repetirTirada.Contains(x)) repetirTirada.Add(x);
                            if (!repetirTirada.Contains(x + 2)) repetirTirada.Add(x + 2);
                        }
                        if (x + 3 < numJugadores && numeroSacadoJugador[x] == numeroSacadoJugador[x + 3])
                        {
                            if (!repetirTirada.Contains(x)) repetirTirada.Add(x);
                            if (!repetirTirada.Contains(x + 3)) repetirTirada.Add(x + 3);
                        }
                    }
                }

                numeroSacadoJugadorOrdenado.Sort(); // Ordenamos de menor a mayor los números sacados por los jugadores
                numeroSacadoJugadorOrdenado.Reverse(); // Le damos la vuelta a la lista para tener la lista ordenada de mayor a menor
                int cont = 0;
                int pos = 0;
                for (int x = 0; x < numJugadores; x++)
                {
                    if (numeroSacadoJugadorOrdenado[x] != -1)
                    {
                        for (int y = 0; y < numJugadores; y++)
                        {
                            if (numeroSacadoJugadorOrdenado[x] == numeroSacadoJugador[y])
                            {
                                cont++; // Aumentamos para ver si se repiten numeros
                                pos = y; // Guardamos la posición para saber que jugador/coche es
                            }
                        }
                        if (cont == 1)
                        {
                            ordenTurnosCochesAux[x] = pos;
                        }
                    }
                    cont = 0; // Reestablecemos el cont para la siguiente iteración
                    pos = 0; // Reestablecemos la pos para la siguiente iteración
                }
                // Metemos en el array de orden los jugadores en las posiciones que haya un -1 para no chafar las que ya hay
                for (int x = 0; x < numJugadores; x++)
                {
                    if (ordenTurnosCoches[x] == -1)
                    {
                        ordenTurnosCoches[x] = ordenTurnosCochesAux[cont];
                        ordenTurnosCochesAux[cont] = -1; // Reestablecemos la posición a -1 para las siguientes iteraciones
                        cont++;
                    }
                }
            }

            //Debug.Log("Numeros Sacados: " + numeroSacadoJugador[0] + " - " + numeroSacadoJugador[1] + " - " + numeroSacadoJugador[2] + " - " + numeroSacadoJugador[3]);
            //Debug.Log("Numeros Sacados Ordenados: " + numeroSacadoJugadorOrdenado[0] + " - " + numeroSacadoJugadorOrdenado[1] + " - " + numeroSacadoJugadorOrdenado[2] + " - " + numeroSacadoJugadorOrdenado[3]);
            //Debug.Log("Orden Turnos Jugadores: " + ordenTurnosCoches[0] + " - " + ordenTurnosCoches[1] + " - " + ordenTurnosCoches[2] + " - " + ordenTurnosCoches[3]);
            //for (int x = 0; x < repetirTirada.Count; x++) Debug.Log("Repite Tirada: " + repetirTirada[x]);

            estadoTirada = 2; // Ponemos el estado a 2 (repetir tirada)
            for (int x = 0; x < numJugadores; x++) numeroSacadoJugador[x] = -1; // Reestauramos el array de numeros que ha sacado el jugador
            for (int x = 0; x < numJugadores; x++) numeroSacadoJugadorOrdenado[x] = -1; // Restauramos el array de numeros ordenado que ha sacado el jugador

            // Si todos los numeros en el array de orden son diferentes de -1 significa que ya están calculados los turnos y que podemos cambiar el estado del juego
            for (int x = 0; x < numJugadores; x++)
            {
                if (ordenTurnosCoches[x] == -1) break;
                if (x == numJugadores - 1)
                {
                    //guardamos en el array de ordenCoches los coches en orden
                    for (int i = 0; i < numJugadores; i++)
                    {
                        cochesOrdenados[i] = cochesJugar[ordenTurnosCoches[i]];
                    }
                    estadoTirada = 3; // Cambiamos el estado de la tirada para mostrar el turno del jugador
                }
            }
        }
    }

    private void mostrarTurnoJugadorEstado0()
    {
        if(estadoTirada == 3)
        {
            tiempo = Time.time + 2f; // Ponemos los segundos que queremos que pasen
            estadoTirada = 4;
        }
        if(estadoTirada == 4)
        {
            if(Time.time > tiempo)
            {
                estadoJuego = 1;
                cambioEstadoJuego = true;
            }
        }
    }

    private void asignarCasilla()
    {
        posCoches[numJugadorActual] += numDado;
        //TODO: comprobar casilla final
    }

    private void activarDadosRepetidos()
    {
        // Activamos los dados que se repiten
        if (estadoTirada == 2 && repetirTirada.Count != 0)
        {
            // Si aun no se ha repetido la tirada se llama a la función
            if (!repetirTiradaInicialEjecutada) StartCoroutine(RepetirTiradaInicial());

            // Si ya se ha repetido la tirada comprobamos si se pulsa espacio (tecla) para que salte el coche
            else if (Input.GetKeyDown(KeyCode.Space) && Time.time > tiempo)
            {
                SaltarCoche(repetirTirada[0]);
                repetirTirada.RemoveAt(0);
                // Si no quedan coches por saltar cambiamos la variable cambiarEstadoTirada a true
                if (repetirTirada.Count == 0) cambiarEstadoTirada = true;
            }
        }
    }

    private void comprobarTurnos()
    {
        // Comprobamos si tenemos que calcular los turnos si los dados estan parados y calculoTirada == true
        // Solo se hace para la primera vez
        if (calculoTirada)
        {
            // Miramos si todos los dados tienen el estado a 0 (parados)
            int cont = 0;
            for (int x = 0; x < numJugadores; x++)
            {
                if (dadosStates[x] == 0) cont++;
                else break;
            }

            // Si todos los dados están parados cambiamos el estado de la tirada
            if (cont == numJugadores)
            {
                estadoTirada = 1;
                calculoTirada = false;
                repetirTiradaInicialEjecutada = false;
            }
        }
    }

    //*************************** FUNCIONES DADO ***************************
    //**********************************************************************

    // Espera hasta que el coche toque el dado
    private IEnumerator WaitFramesDado(int frames, int dado) 
    {
        yield return new WaitForFrames(frames);
        PararDado(dado);
    }

    // Activamos la animación de un dado
    public void MoverDado(GameObject dadoActual)
    {
        dadoActual.transform.GetChild(0).GetComponent<Animator>().SetBool("movimiento", true);
        dadosStates[ordenTurnosCoches[numJugadorActual]] = 1; // Ponemos el estado del dado a animación activa
        tiempo = Time.time + 1f; // Ponemos los segundos que queremos que pasen
    }

    // Pone una rotación al dado seleccionado y se para la animacion de girar_dado
    public void PararDado(int dado) 
    {
        if(estadoJuego == 0)
        {
            if (dadosStates[dado] != 0)
            {
                //Debug.Log("Paramos el dado");
                int rand = Random.Range(0, 6);  // Sacar numero random
                numeroSacadoJugador[dado] = rand + 1; // Guardamos para el jugador el número que ha salido en el dado
                numeroSacadoJugadorOrdenado[dado] = rand + 1; // Guardamos para el jugador el número que ha salido en el dado y después ordenarlo
                dadosJugar[dado].transform.localRotation = rotDados[rand]; // Seleccionamos la cara del dado en el array
                //dadosAnim[dado].ReSetBool("movimiento",true);
                dadosAnim[dado].SetBool("movimiento", false);
                dadosStates[dado] = 0;
                if (cambiarEstadoTirada) // Cambiamos el estado de la Tirada a 1(calcular turnos) si la variable es true
                {
                    cambiarEstadoTirada = false;
                    estadoTirada = 1;
                    repetirTiradaInicialEjecutada = false;
                }
            }
        }
        else if(estadoJuego == 1 || estadoJuego == 4)
        {
            if (dadosStates[dado] != 0)
            {
                int rand = Random.Range(0, 6);  // Sacar numero random
                dadosJugar[dado].transform.localRotation = rotDados[rand]; // Seleccionamos la cara del dado en el array
                numDado = rand + 1;
                dadosAnim[dado].SetBool("movimiento", false);
                dadosStates[dado] = 0;
                StartCoroutine(llamarMovCoche());
            }   
        }

    }

    // Activamos los dados que se repiten
    private IEnumerator RepetirTiradaInicial()
    {
        yield return new WaitForSeconds(1.5f);
        for (int x = 0; x < repetirTirada.Count; x++)
        {
            if (dadosStates[repetirTirada[x]] == 0)
            {
                //Debug.Log("Activo el dado " + repetirTirada[x]);
                dadosAnim[repetirTirada[x]].SetBool("movimiento",true);
                dadosStates[repetirTirada[x]] = 1;
            }
        }
        repetirTiradaInicialEjecutada = true;
        tiempo = Time.time + 0.05f; // Ponemos los segundos que queremos que pasen
    }

    //*************************** FUNCIONES COCHE ***************************
    //***********************************************************************

    // Espera hasta que el coche acabe la animacion
    private IEnumerator WaitFramesCoche(int frames, int coche)
    {
        yield return new WaitForFrames(frames);
        PararCoche(coche);
    }

    // Activa la animación del coche seleccionado y le cambiamos el estado en el array de estados
    private void SaltarCoche(int coche) 
    {
        cochesAnim[coche].SetTrigger("start");
        cochesStates[coche] = 1;
        StartCoroutine(PararCoche(coche));
    }

    // Paramos la animacion del coche y reestablecemos la animacion de saltar
    private IEnumerator PararCoche(int coche)
    {
        yield return new WaitForSeconds(1.0f);
        //cochesAnim[coche].ReSetBool("movimiento",true);
        cochesAnim[coche].SetTrigger("stop");
        cochesStates[coche] = 0;
    }
    public void tiradaCoche(GameObject cocheActual)
    {        
        //cambiamos de camara
        camaraPrincipal.enabled = false;

        //activamos su camara y su dado y lanzamos su dado
        foreach (Transform child in cocheActual.transform)
        {

            if (child.tag == "camCars")
                child.gameObject.SetActive(true);
            else if (child.tag == "dadoCoche")
            {
                child.gameObject.SetActive(true);
                dadoActual = child.gameObject;
                MoverDado(dadoActual);
            }
        }
    }

    //comprobamos que tipo de casilla ha caido
    public void comprobarCasilla(string casilla, int numCasilla)
    {
        if (casilla == "Juego" && mainMenu.Level == "facil")
        {
            escenaTablero.SetActive(false);
            estadoJuego = 2; //estado del juego facil
            escenaJF.SetActive(true);
            gManagerFacil.asignarNivel(numCasilla);
        }
        else if (casilla == "Juego" && mainMenu.Level == "dificil")
        {
            escenaTablero.gameObject.SetActive(false);
            estadoJuego = 3; //estado del juego dificil
            escenaJD.SetActive(true);
            gManagerScratch.startGame(numCasilla);
        }
        else if(casilla == "exclamacion")
        {
            // se sumarán x puntos al coche que ha caido en esta casilla
            ruleta.SetActive(true); //se activa con el metodo onEnable de spin.cs
        }
        else if(casilla == "Preguntas")
        {
            // Activamos el estado preguntas
            estadoJuego = 6;

            // Mostramos la pregunta y las respuestas
            preguntaCanvas.mostrarPreguntaYRespuesta(numCasilla);
        }
        else if(casilla == "ultimaCasilla")
        {
            // Desactivamos la cámara solo si no es el último jugador que queda
            if(numJugadoresEliminados.Count < numJugadores - 1)
                desactivarCamaras();

            // Se mostrará el ganador con sus puntos (cuando no queden jugadores por llegar a la meta)
            switch (numJugadoresEliminados.Count)
            {
                case 0:
                    siguienteJugador(300, true);
                    break;

                case 1:
                    siguienteJugador(200, true);
                    break;

                case 2:
                    siguienteJugador(100, true);
                    break;

                case 3:
                    siguienteJugador(50, true);
                    break;

                default:
                    break;
            }
        }
    }

    public void desactivarCamaras()
    {
        foreach (Transform child in cochesOrdenados[numJugadorActual].transform)
        {
            if (child.tag == "camCars")
                child.gameObject.SetActive(false);
            else if (child.tag == "dadoCoche")
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    //Cambiar estado, coche, camara...Para pasar al siguiente jugador si es que hay
    public void siguienteJugador(int puntos, bool eliminarJugador)
    {
        // Sumamos los puntos al coche actual
        puntosCoches[ordenTurnosCoches[numJugadorActual]] += puntos;

        //canvasPuntos.transform.GetChild(numJugadorActual).GetComponent<Image>().color = new Color(0, 0, 0, 0); // Desactivamos la selección del jugador actual
        // Cambiamos el color de los textos a blanco del jugador activo
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(0).GetComponent<UnityEngine.UI.Text>().color = Color.white;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(2).GetComponent<UnityEngine.UI.Text>().color = Color.white;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(3).GetComponent<UnityEngine.UI.Text>().color = Color.white;

        // Si hay que eliminar a algún jugador lo eliminamos, si no avanzamos un jugador
        if(eliminarJugador)
        {
            numJugadoresEliminados.Add(numJugadorActual);
        }
        else
            numJugadorActual++;

        // Si llegamos al máximo de jugadores activos volvemos al inicio
        if (numJugadorActual == numJugadores)
        {
            numJugadorActual = 0;
        }

        while(numJugadoresEliminados.Contains(numJugadorActual) && numJugadoresEliminados.Count < numJugadores)
        {
            numJugadorActual++;

            // Si llegamos al máximo de jugadores activos volvemos al inicio
            if (numJugadorActual == numJugadores)
            {
                numJugadorActual = 0;
            }
        }

        //canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetComponent<Image>().color = new Color(0, 0, 0, 100f / 255f); // Activamos la selección del nuevo jugador actual
        // Cambiamos el color de los textos a negro del nuevo jugador activo
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(0).GetComponent<UnityEngine.UI.Text>().color = Color.black;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(2).GetComponent<UnityEngine.UI.Text>().color = Color.black;
        canvasPuntos.transform.GetChild(ordenTurnosCoches[numJugadorActual]).GetChild(3).GetComponent<UnityEngine.UI.Text>().color = Color.black;

        // Si el número de jugadores eliminados es igual al número de jugadores ponemos el estado de fin de juego, sino el estado de tirada del siguiente jugador y se llama a la función de tirada
        if (numJugadoresEliminados.Count >= numJugadores)
        {
            canvasPuntos.SetActive(true); // Activamos el canva de puntos
            imprimirPuntosFinal(); // Imprimimos los puntos de los jugadores por última vez
            estadoJuego = 7;
        }
        else
        {
            estadoJuego = 1;
            tiradaCoche(cochesOrdenados[numJugadorActual]);
        }
    }
}