using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public List<int> codigo = new List<int>();
    private Coche coche;
    private Vector3 positionInit;
    private Quaternion rotationInit;


    public Vector3 PositionInit { get => positionInit; set => positionInit = value; }
    public Quaternion RotationInit { get => rotationInit; set => rotationInit = value; }

    private void OnEnable()
    {
        mover();
        positionInit = this.transform.position;
        rotationInit = this.transform.rotation;
    }

    public void mover()
    {
        int i = codigo[0];
        if (i == 1)
        {

            Vector3 target = transform.GetChild(0).position;
            StartCoroutine(LerpPosition(target, 1));
        }
        //left
        if (i == 2)
        {
            Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
            StartCoroutine(LerpRotation(rot, 1));
        }
        //right
        if (i == 3)
        {
            Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
            StartCoroutine(LerpRotation(rot, 1));

        }
        codigo.RemoveAt(0);
        codigo.Add(i);
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
}
