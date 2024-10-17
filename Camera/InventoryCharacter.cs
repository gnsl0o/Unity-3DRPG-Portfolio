using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCharacter : MonoBehaviour
{
    public Camera cam;
    public RawImage rawImage;
    public GameObject character;
    public RenderTexture inventoryRenderTexture;

    private void Start()
    {
        cam.targetTexture = inventoryRenderTexture;
        rawImage.texture = inventoryRenderTexture;

        cam.gameObject.SetActive(false);
        character.SetActive(false);
    }

    public void OpenInventory()
    {
        cam.gameObject.SetActive(true);
        character.SetActive(true);
    }

    public void CloseInventory()
    {
        cam.gameObject.SetActive(false);
        character.SetActive(false);
    }
}
