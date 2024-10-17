using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // �÷��̾ �þ߿� �����Ǿ����� Ȯ���ϴ� �̺�Ʈ
    public event Action<bool, GameObject> OnVisibilityChanged;

    // �þ� ������ �������� �þ� ����
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    // ����ũ 2��
    public LayerMask targetMask, obstacleMask;

    // Target mask�� ray hit�� transform�� �����ϴ� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    // �ӽ�
    private GameObject player;

    private void Start()
    {
        // 0.2�� �������� �ڷ�ƾ ȣ��
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        bool previousVisible = visibleTargets.Count > 0;
        visibleTargets.Clear();
        // viewRadius�� ���������� �� �� ���� �� targetMask ���̾��� �ݶ��̴��� ��� ������
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);


        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // �÷��̾�� forward�� target�� �̷�� ���� ������ ���� �����
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Ÿ������ ���� ����ĳ��Ʈ�� obstacleMask�� �ɸ��� ������ visibleTargets�� Add
                if(!Physics.Raycast(transform.position, dirToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        // previousVisible ������ ������� �ʴٸ� OnVisibilityChanged �̺�Ʈ ȣ��
        if(previousVisible != (visibleTargets.Count > 0))
        {
            if (visibleTargets.Count > 0)
            {
                player = visibleTargets[0].gameObject; // ���� ��� ù ��° ����� �����մϴ�.
            }
            OnVisibilityChanged?.Invoke(visibleTargets.Count > 0, player);
        }
    }

    public Vector3 DirFromAngle(float angleDegress, bool angleGlobal)
    {
        if (!angleGlobal)
        {
            angleDegress += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin((-angleDegress + 90) * Mathf.Deg2Rad), 0, Mathf.Cos(-angleDegress + 90) * Mathf.Deg2Rad);
    }

    public void SetViewDistance(float newAngle, float newDistance)
    {
        viewAngle = newAngle;
        viewRadius = newDistance;
    }

    void OnDrawGizmos()
    {
        Vector3 forward = transform.forward; // ĳ������ ���� ����
        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up); // ���� ��輱 ����
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2, Vector3.up); // ������ ��輱 ����
        Vector3 leftRayDirection = leftRayRotation * forward; // ���� ��輱 ����
        Vector3 rightRayDirection = rightRayRotation * forward; // ������ ��輱 ����

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftRayDirection * viewRadius); // ���� ��輱 �׸���
        Gizmos.DrawRay(transform.position, rightRayDirection * viewRadius); // ������ ��輱 �׸���
    }
}
