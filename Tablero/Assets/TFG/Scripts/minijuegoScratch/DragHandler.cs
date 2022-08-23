using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // Variables
    public static GameObject itemDragging;
    private Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

        // Guardamos en itemDragging el objeto que vamos a arrastrar
        itemDragging = gameObject;

        // Guaramos la posici�n inicial del objeto que vamos a arrastrar
        startPosition = transform.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        // Actualizamos la posici�n del objeto seg�n la posici�n del rat�n
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");

        // Ponemos a null nuestro itemDragging
        itemDragging = null;

        // Reestablecemos la posici�n del objeto
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
