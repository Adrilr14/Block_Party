using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    // Scripts
    public GameManagerScratch gms;

    public void OnDrop(PointerEventData eventData)
    {
        if (DragHandler.itemDragging)
        {
            // Creamos un nuevo botón según el tag de la pieza (pasamos el GameObject por parámetro)
            gms.addPiezaButton(DragHandler.itemDragging);
        }
    }
}
