using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDeActivator : MonoBehaviour, IPointerExitHandler
{

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);    
    }
}
