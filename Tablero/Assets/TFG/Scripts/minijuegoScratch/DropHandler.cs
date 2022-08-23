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
            // Creamos un nuevo bot�n seg�n el tag de la pieza (pasamos el GameObject por par�metro)
            gms.addPiezaButton(DragHandler.itemDragging);
        }
    }
}
