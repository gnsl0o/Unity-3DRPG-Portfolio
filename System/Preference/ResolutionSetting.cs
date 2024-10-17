using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSetting : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct(new ResolutionComparer()).ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width) &&
                resolutions[i].height == PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex; // 드롭다운 메뉴에서 현재 화면 해상도를 기본 선택 옵션으로 설정
        resolutionDropdown.RefreshShownValue(); // 드롭다운 메뉴에 표시되는 값을 갱신하여 현재 선택된 옵션을 보여줌
        
        // 저장된 해상도 적용
        SetResolution(currentResolutionIndex);
    }



    // 드롭다운에서 선택한 값으로 해상도를 결정하는 함수
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log(Screen.width + " * " +  Screen.height);

        // 선택한 해상도를 저장
        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
        PlayerPrefs.Save();
    }

    class ResolutionComparer : IEqualityComparer<Resolution>
    {
        public bool Equals(Resolution x, Resolution y)
        {
            return x.width == y.width && x.height == y.height;
        }

        public int GetHashCode(Resolution obj)
        {
            return obj.width.GetHashCode() ^ obj.height.GetHashCode();
        }
    }
}
