using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Coche : MonoBehaviour
{
    /*private int up = 1;
    private int left = 2;
    private int right = 3;
    */

    public Collision collision; 
    private bool canMove;
    private bool collideEnemy;
    private bool finish;
    private bool rueda;
    public GameObject ruedaJuego;
    private bool go;
    private List<int> codigo = new List<int>();
    // Adjust the speed for the application.
    public float speed;
    // The target (cylinder) position.
    private Vector3 target;
    private Quaternion rot;
    private List<int> codCopy;

    //bool para saber si ha colisionado con el final
    //private bool testFin; NO SE USABA
    //botones scroll
    public GameObject buttonPrefabUp;
    public GameObject buttonPrefabLeft;
    public GameObject buttonPrefabRight;
    public GameObject scroll;
    public Scrollbar scrollbar;

    //posicion del coche inicial
    private Quaternion rotCocheIni;
    private Vector3 posCocheIni;

    public GameManagerLna cam; //

    private void Start()
    {
        rotCocheIni = transform.rotation;
        posCocheIni = transform.position;
        
    }

    public void ResetCoche()
    {
        transform.rotation = rotCocheIni;
        transform.position = posCocheIni;
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
        target = transform.position + (transform.TransformDirection(Vector3.down) * 5);
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
    public void startMovement()
    {
        codCopy = new List<int>(codigo);
        mover();
    }
    public void addMovement(int movimiento)
    {
        codigo.Add(movimiento);
        GameObject btn;
        switch (movimiento)
        {
            case 1:
                btn = buttonPrefabUp;
                break;
            case 2:
                btn = buttonPrefabLeft;
                break;
            case 3:
                btn = buttonPrefabRight;
                break;
            default:
                btn = buttonPrefabUp;
                break;
        }
        cam.sonarSelect();
        GameObject button = Instantiate(btn, scroll.transform) as GameObject;
        button.name = scroll.transform.childCount.ToString();
        button.GetComponent<Button>().onClick.AddListener(() => DeleteButton());
        StartCoroutine(resetScrollbar());
    }   
    public IEnumerator resetScrollbar()
    {
        yield return new WaitForSeconds(0.1f);
        scrollbar.value = 1;
    }

    private void DeleteButton()
    {
        string nameButton = EventSystem.current.currentSelectedGameObject.name;
        int num = int.Parse(nameButton) - 1;

        for (int i = num; i < scroll.transform.childCount; i++)
        {
            scroll.transform.GetChild(i).name = i.ToString();
        }
        Destroy(scroll.transform.GetChild(num).gameObject);
        cam.sonarDelete();
        codigo.RemoveAt(num);
    }
    
    public void deleteCode()
    {
        if (codigo.Count > 0) 
        {
            foreach (Transform child in scroll.transform)
            {
                Destroy(child.gameObject);
            }
            cam.sonarDelete();
            codigo.Clear();
        }
        else
        {
            cam.sonarError();
        }
    }
    public void mover()
    {
        if (codCopy.Count > 0)
        {
            int i = codCopy[0];
            if(i == 1 && canMove)
            {

                target = transform.GetChild(0).position;
                StartCoroutine(LerpPosition(target, .5f));
            }
            //left
            if (i == 2)
            {
                rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 90);
                StartCoroutine(LerpRotation(rot, 1));
            }
            //right
            if(i == 3)
            {
                rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90);
                StartCoroutine(LerpRotation(rot, 1));

            }
            codCopy.RemoveAt(0);
            if (!canMove && i == 1)
            {
                target = transform.position + (transform.TransformDirection(Vector3.up) * 5);
                StartCoroutine(LerpPositionMal(target, .5f));
                
            }
        }
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

        bool nivelEspecial = false;
        // Does the ray intersect any objects excluding the player layer
        canMove = Physics.Raycast(transform.GetChild(0).position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity, layerMask);
        finish = Physics.Raycast(transform.GetChild(1).position, transform.TransformDirection(Vector3.back), out hit2, Mathf.Infinity, layerMask);

        

        if (cam.timerIsRunning && hit2.transform.gameObject.CompareTag("Finish"))
        {
            deleteCode();
            finish = true;
            if(cam.Nivel == 2 || cam.Nivel == 3 || cam.Nivel == 5)
            {
                nivelEspecial = true;
                cam.comprobarRueda();
            }
            else
            {
                cam.FinJuegoPasado();
            }

        }
        if (cam.TimeRemaining == 0f && !nivelEspecial)
        {
            finish = true;
            cam.FinJuegoSinPasar();
        }

    }
    //volver a empezar desde el principio del mapa borrando el código
    public void StartAgain()
    {
        if(codigo != null)
        {
            StopAllCoroutines();
            transform.position = posCocheIni;
            transform.rotation = rotCocheIni;
            deleteCode();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("rueda"))
        {
            cam.Rueda++;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemigo"))
        {
            StartAgain();
        }
    }
}
