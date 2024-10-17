using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preference : MonoBehaviour
{
    // 캡처용 카메라와 게임 플레이 카메라를 참조
    public Camera mainCamera;
    public Camera captureCamera;
    public RenderTexture renderTexture;
    public Image displayImage;

    // 화면 캡처 전에 호출
    void CaptureScreen()
    {
        // 게임 플레이 카메라의 위치와 회전을 캡처용 카메라에 복사
        captureCamera.transform.position = mainCamera.transform.position;
        captureCamera.transform.rotation = mainCamera.transform.rotation;

        captureCamera.gameObject.SetActive(true);
        StartCoroutine(CaptureIt());
    }

    IEnumerator CaptureIt()
    {
        // 한 프레임 기다림. 모든 렌더링이 완료될 때까지 기다리기 위함
        yield return new WaitForEndOfFrame();

        captureCamera.Render();

        // RenderTexture를 Texture2D로 변환
        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenShot.Apply();

        // 사용이 끝난 후 카메라의 targetTexture를 정리하지 않습니다. 미리 만든 RenderTexture를 계속 사용할 수 있기 때문입니다.
        RenderTexture.active = null; // RenderTexture가 더 이상 활성 상태가 아님을 명시

        // 캡처용 카메라를 비활성화
        captureCamera.gameObject.SetActive(false);

        // Texture2D를 Sprite로 변환
        Sprite screenshotSprite = CreateSpriteFromTexture(screenShot);

        // UI Image에 적용
        displayImage.sprite = screenshotSprite;

        Destroy(screenShot);
    }

    Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
