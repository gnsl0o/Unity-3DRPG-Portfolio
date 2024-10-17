using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBuffer : MonoBehaviour
{
    private PlayerInput playerInput;
    private string lastInput;
    private float lastInputTime;
    private float bufferTime = 0.7f;

    private Coroutine inputTimerCoroutine;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.OnInputReceived += BufferInput;
    }

    private void OnDestroy()
    {
        playerInput.OnInputReceived -= BufferInput;
    }

    private void BufferInput(string input)
    {
        lastInput = input; // �Է� �ޱ�
        // ������ ���� ���� Coroutine�� �ִٸ� ����
        if (inputTimerCoroutine != null)
        {
            StopCoroutine(inputTimerCoroutine);
        }
        // Coroutine ����
        inputTimerCoroutine = StartCoroutine(ResetInputAfterTime(bufferTime));
    }

    private IEnumerator ResetInputAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // ������ �ð���ŭ ���
        lastInput = null; // �ð��� ������ �Է� �ʱ�ȭ
        inputTimerCoroutine = null; // Coroutine ���� �ʱ�ȭ
    }

    public string GetLastInput()
    {
        return lastInput; // �ֱ� �Է� ��ȯ
    }

    public void ClearLastInput()
    {
        lastInput = null;
    }
}