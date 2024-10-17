using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public GameObject image;

    public void OnMouseEnter()
    {
        image.transform.position = transform.position;
        image.SetActive(true);
    }

    public void OnMouseExit()
    {
        image.SetActive(false);
    }
}
