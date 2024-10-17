using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActive : MonoBehaviour
{
    public Collider attackCollider; // 공격에 사용될 콜라이더

    public Collider bodyCollider; // 킬무브시 사라질 콜라이더

    // Animation Event로 호출될 함수
    public void ActivateCollider()
    {
        attackCollider.enabled = true; // 콜라이더 활성화
    }

    // Animation Event로 호출될 함수
    public void DeactivateCollider()
    {
        attackCollider.enabled = false; // 콜라이더 비활성화
    }

    public void EnableBodyCollider()
    {
        bodyCollider.enabled = true;
    }
    public void DeactiveBodyCollider()
    {
        bodyCollider.enabled = false;
    }
}
