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
        lastInput = input; // 입력 받기
        // 기존에 실행 중인 Coroutine이 있다면 중지
        if (inputTimerCoroutine != null)
        {
            StopCoroutine(inputTimerCoroutine);
        }
        // Coroutine 시작
        inputTimerCoroutine = StartCoroutine(ResetInputAfterTime(bufferTime));
    }

    private IEnumerator ResetInputAfterTime(float time)
    {
        yield return new WaitForSeconds(time); // 지정된 시간만큼 대기
        lastInput = null; // 시간이 지나면 입력 초기화
        inputTimerCoroutine = null; // Coroutine 참조 초기화
    }

    public string GetLastInput()
    {
        return lastInput; // 최근 입력 반환
    }

    public void ClearLastInput()
    {
        lastInput = null;
    }
}