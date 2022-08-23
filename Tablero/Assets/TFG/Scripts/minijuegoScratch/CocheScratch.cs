using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocheScratch : MonoBehaviour
{
    // Scripts
    public GameManagerScratch gms;

    // Variables script
    public Collision collision;
    public float speed; // Adjust the speed for the application.
    private bool canMove;
    private bool collideEnemy;
    private bool finish;
    private bool rueda;
    private bool arrancado;
    private List<int> repetirCodigoList; // Lista para guardar las el número de repetición de las piezas For init
    private List<int> repetirCodigoListCopy; // Lista para guardar las el número de repetición de las piezas For init
    private List<int> repetirCodigoListAux; // Variable auxiliar para añadir 0 al repetir código cuando se ñadan movimientos en el For Init
    //private int idxRepetirCodigoList; // Variable para guardar el index en el que nos encontramos de la lista repetir codigo
    private List<int> codigo; // 1 = Arrancar (On); 2 = Avanzar; 3 = Left; 4 = Right; 5 = For init; 6 = For end; 7 = Parar motor (Off);
    private List<int> codigoCopy; // Variable para guardar una copia del código y que no pase nada si se elimina durante el movimiento
    private List<int> codigoAux; // Variable para guardar el código para insertar después de un For init
    private Vector3 target; // The target (cylinder) position.
    private Quaternion rot;

    // Variables transform
    private Quaternion rotCocheIni;
    private Vector3 posCocheIni;

    // Objetos
    public GameObject ruedaJuego;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializamos algunas variables
        canMove = false;
        finish = false;
        arrancado = false;
        repetirCodigoList = new List<int>();
        repetirCodigoListAux = new List<int>();
        codigo = new List<int>();
        codigoAux = new List<int>();
        //idxRepetirCodigoList = 0;
        

        // Guardamos la posiciçon y la rotación inicial del coche
        rotCocheIni = transform.rotation;
        posCocheIni = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        RaycastHit hit2;

        // Does the ray intersect any objects excluding the player layer
        canMove = Physics.Raycast(transform.GetChild(0).position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity, layerMask);
        finish = Physics.Raycast(transform.GetChild(1).position, transform.TransformDirection(Vector3.back), out hit2, Mathf.Infinity, layerMask);

        if (gms.timerIsRunning && finish && hit2.transform.gameObject.CompareTag("Finish") && !arrancado)
        {
            deleteCode();
            finish = true;
            gms.finJuego();

        }
        else if (!gms.timerIsRunning && gms.TimeRemaining <= 0f)
        {
            finish = true;
            gms.finJuegoSinPasar();
        }

    }

    public void resetCoche()
    {
        transform.rotation = rotCocheIni;
        transform.position = posCocheIni;
    }

    /*public void addMovement(int movimiento)
    {
        codigo.Add(movimiento);
    }*/

    public void addMovement(int movimiento, int vecesRepetir)
    {
        codigo.Add(movimiento);
        repetirCodigoList.Add(vecesRepetir);
    }

    public void deleteMovement(int movimiento)
    {
        codigo.RemoveAt(movimiento);
        repetirCodigoList.RemoveAt(movimiento);
    }

    public void deleteCode()
    {
        codigo.Clear();
        repetirCodigoList.Clear();
    }

    // Empezar desde el principio del mapa borrando el código
    public void startAgain()
    {
        if (codigo != null)
        {
            arrancado = false;
            StopAllCoroutines();
            transform.position = posCocheIni;
            transform.rotation = rotCocheIni;
            gms.deleteAllButtons();
        }

    }

    public void startMovement()
    {
        codigoCopy = new List<int>(codigo);
        repetirCodigoListCopy = new List<int>(repetirCodigoList);
        mover();
    }

    // Movemos el coche por las casillas. Solo se moverá si está arrancado y podrá finalizar si se para el motro del coche.
    public void mover()
    {
        if (codigoCopy.Count > 0)
        {
            gms.timerIsRunning = false; // Paramos el cronómetro
            int i = codigoCopy[0];
            bool llamarMover = false; // Variable para llamar a la función mover cuando el coche no se mueve

            // Arrancar (On)
            if (i == 1)
            {
                arrancado = true;
                llamarMover = true;
            }
            // Avanzar
            else if (i == 2 && arrancado)
            {
                if (canMove)
                {
                    target = transform.GetChild(0).position;
                    StartCoroutine(LerpPosition(target, .5f));
                }
                else
                {
                    target = transform.position + (transform.TransformDirection(Vector3.up) * .01f);
                    StartCoroutine(LerpPositionMal(target, .5f));
                }
            }
            // Left
            else if (i == 3 && arrancado)
            {
                rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
                StartCoroutine(LerpRotation(rot, 1));
            }
            // Right
            else if (i == 4 && arrancado)
            {
                rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90);
                StartCoroutine(LerpRotation(rot, 1));

            }
            // For init
            else if (i == 5 && arrancado && repetirCodigoListCopy[0] > 0)
            {
                llamarMover = true;

                for (int j = 1; j < codigoCopy.Count; j++)
                {
                    if (codigoCopy[j] != 6)
                    {
                        codigoAux.Add(codigoCopy[j]);
                        repetirCodigoListAux.Add(0);
                    }
                    else
                        break;
                }
                for (int j = 0; j < repetirCodigoListCopy[0]-1; j++)
                {
                    codigoCopy.InsertRange(1, codigoAux);
                    repetirCodigoListCopy.InsertRange(1, repetirCodigoListAux);
                }
                
                codigoAux.Clear(); // Limpiamos la lista codigoAux
                repetirCodigoListAux.Clear(); // Limpiamos la lista repetirCodigoListAux
            }
            // For end
            else if (i == 6)
            {
                llamarMover = true;
            }
            // Parar motor (Off)
            else if (i == 7)
            {
                arrancado = false;
                llamarMover = true;
            }

            // Borramos del código de copia el movimiento actual
            codigoCopy.RemoveAt(0);

            // Borramos del código de copia el valor actual de repetición del código
            repetirCodigoListCopy.RemoveAt(0);

            // Aumentamos el index de la lista repetir codigo
            //idxRepetirCodigoList++;
            //if (idxRepetirCodigoList >= repetirCodigoListCopia.Count) { idxRepetirCodigoList = 0; }

            // LLamamos a la función mover para que continue el código
            if (llamarMover)
            {
                llamarMover = false;
                mover();
            }
        }
        else
        {
            gms.timerIsRunning = true; // Activamos el cronómetro
            //idxRepetirCodigoList = 0;
        }
    }

    //mover posicion
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mover();
        //transform.position = targetPosition;
    }

    //mover cuando no puede
    IEnumerator LerpPositionMal(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        target = transform.position + (transform.TransformDirection(Vector3.down) * .01f);
        StartCoroutine(LerpPosition(target, .5f));
    }

    //rotacion
    IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
        mover();
    }

}
