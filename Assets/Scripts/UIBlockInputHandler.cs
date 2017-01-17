using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBlockInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData)
    {
        InputHandler.numBlockers++;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputHandler.numBlockers--;
    }
}
