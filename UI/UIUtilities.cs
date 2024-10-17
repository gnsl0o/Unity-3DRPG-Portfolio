using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UIUtilities
{
    // 게임 오브젝트의 활성화 상태를 설정하는 유틸리티 함수
    public static void SetUIActive(GameObject uiObject, bool isActive)
    {
        if(uiObject != null)
        {
            uiObject.SetActive(isActive);
        }
    }
}