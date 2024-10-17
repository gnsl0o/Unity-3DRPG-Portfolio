using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preference : MonoBehaviour
{
    // ĸó�� ī�޶�� ���� �÷��� ī�޶� ����
    public Camera mainCamera;
    public Camera captureCamera;
    public RenderTexture renderTexture;
    public Image displayImage;

    // ȭ�� ĸó ���� ȣ��
    void CaptureScreen()
    {
        // ���� �÷��� ī�޶��� ��ġ�� ȸ���� ĸó�� ī�޶� ����
        captureCamera.transform.position = mainCamera.transform.position;
        captureCamera.transform.rotation = mainCamera.transform.rotation;

        captureCamera.gameObject.SetActive(true);
        StartCoroutine(CaptureIt());
    }

    IEnumerator CaptureIt()
    {
        // �� ������ ��ٸ�. ��� �������� �Ϸ�� ������ ��ٸ��� ����
        yield return new WaitForEndOfFrame();

        captureCamera.Render();

        // RenderTexture�� Texture2D�� ��ȯ
        RenderTexture.active = renderTexture;
        Texture2D screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenShot.Apply();

        // ����� ���� �� ī�޶��� targetTexture�� �������� �ʽ��ϴ�. �̸� ���� RenderTexture�� ��� ����� �� �ֱ� �����Դϴ�.
        RenderTexture.active = null; // RenderTexture�� �� �̻� Ȱ�� ���°� �ƴ��� ���

        // ĸó�� ī�޶� ��Ȱ��ȭ
        captureCamera.gameObject.SetActive(false);

        // Texture2D�� Sprite�� ��ȯ
        Sprite screenshotSprite = CreateSpriteFromTexture(screenShot);

        // UI Image�� ����
        displayImage.sprite = screenshotSprite;

        Destroy(screenShot);
    }

    Sprite CreateSpriteFromTexture(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
