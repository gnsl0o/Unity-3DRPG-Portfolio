using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderActive : MonoBehaviour
{
    public Collider attackCollider; // ���ݿ� ���� �ݶ��̴�

    public Collider bodyCollider; // ų����� ����� �ݶ��̴�

    // Animation Event�� ȣ��� �Լ�
    public void ActivateCollider()
    {
        attackCollider.enabled = true; // �ݶ��̴� Ȱ��ȭ
    }

    // Animation Event�� ȣ��� �Լ�
    public void DeactivateCollider()
    {
        attackCollider.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
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
