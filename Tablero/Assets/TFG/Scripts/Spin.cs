using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    private float randomvalue;
    private bool spinning;
    private float finalAngle;

    public GameObject spin; // Ruleta
    public GameManager gManager; //script

    //velocidad de la rueda
    private int initialSpeed = 200;
    private float speed;

    private int puntos = 0;
    [SerializeField]
    private Text PremioText;

    //estados iniciales
    private Vector3 spinPosition;
    private Quaternion spinRotation;

    // Use this for initialization
    private void Start()
    {
        OnEnable();
    }
    private void Update()
    {
        if (spinning) spin.transform.GetChild(1).Rotate(0, 0, Time.deltaTime * speed);
        else Rueda();
        if (Input.GetKeyDown(KeyCode.Space)) spinning = false;
    }
    private void OnEnable()
    {
        spinning = true;
        speed = initialSpeed;
        spinPosition = spin.transform.GetChild(1).position;
        spinRotation = spin.transform.GetChild(1).rotation;
        randomvalue = 0;
    }

    private void Rueda()
    {
        if (randomvalue == 0)
        {
            randomvalue = Random.Range(0.5f, 1.0f);
        }
        else if(speed > 0)
        {
            speed -= randomvalue;
            if(speed < 0)
            {
                speed = 0;
            }
        }

        spin.transform.GetChild(1).Rotate(0, 0, Time.deltaTime * speed);
        if(speed <= 0)
        {
            //if (Mathf.RoundToInt(spin.transform.GetChild(1).eulerAngles.z) % 45 != 0) spin.transform.GetChild(1).Rotate(0, 0, Time.deltaTime * speed);

            //finalAngle = Mathf.RoundToInt(spin.transform.GetChild(1).eulerAngles.z);
            finalAngle = spin.transform.GetChild(1).eulerAngles.z;

            if(finalAngle >= 337.5f || finalAngle < 22.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "¡Ganas 100 puntos!";
                puntos = 100;
            }
            else if (finalAngle >= 22.5f && finalAngle < 67.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Ganas 25 puntos";
                puntos = 25;
            }
            else if (finalAngle >= 67.5f && finalAngle < 112.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Ganas 50 puntos";
                puntos = 50;
            }
            else if (finalAngle >= 112.5f && finalAngle < 157.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "No ganas ni pierdes puntos";
                puntos = 0;
            }
            else if (finalAngle >= 157.5f && finalAngle < 202.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Ganas 25 puntos";
                puntos = 25;
            }
            else if (finalAngle >= 202.5f && finalAngle < 247.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Ganas 50 puntos";
                puntos = 50;
            }
            else if (finalAngle >= 247.5f && finalAngle < 292.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Ganas 25 puntos";
                puntos = 25;
            }
            else if (finalAngle >= 292.5f && finalAngle < 337.5f)
            {
                PremioText.gameObject.SetActive(true);
                PremioText.text = "Pierdes 50 puntos...";
                puntos = -50;
            }

            StartCoroutine(esperarSegundos(puntos)); // Esperamos un tiempo para que el jugador pueda leer el mensaje
        }
    }

    private IEnumerator esperarSegundos(int puntos)
    {
        yield return new WaitForSeconds(2);
        PremioText.gameObject.SetActive(false);
        spin.SetActive(false);
        gManager.desactivarCamaras();
        gManager.siguienteJugador(puntos, false);
    }
}