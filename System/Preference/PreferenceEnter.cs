using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceEnter : MonoBehaviour
{
    public GameObject preference_Canvas;
    public GameObject disable_Canvas;

    public void SetPreference()
    {
        preference_Canvas.SetActive(true);
        disable_Canvas.SetActive(false);
    }
}
