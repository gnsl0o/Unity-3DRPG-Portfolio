using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LockOn : MonoBehaviour
{
    public GameObject marker; // ���� ��Ŀ
    private GameObject currentMarker; // ���� Ȱ��ȭ�� ��Ŀ ������Ʈ
    public GameObject parryMarker;

    public CinemachineVirtualCamera originalCamera;
    public CinemachineVirtualCamera virtualCamera;
    public Camera cam;
    public bool isLockOn = false;
    public LayerMask enemyLayer;

    public Transform playerTransform;

    public float detectionRadius = 10f;
    public float frontOffset = 5f;

    private EnemyHealth closestEnemy; // ���� ����� �����ϴ� ����

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (isLockOn)
            {
                DeactivateLockOn();
            }
            else
            {
                closestEnemy = FindClosestEnemy();

                if (closestEnemy != null)
                {
                    Debug.Log("�߰ߵ� �� = " +  closestEnemy.name);
                    ActivateLockOn(closestEnemy);
                    transform.LookAt(closestEnemy.transform.position);
                }
            }
        }

        if (isLockOn && closestEnemy != null)
        {
            Transform markerPosition = closestEnemy.marker;
            marker.transform.position = markerPosition.position;

            marker.transform.LookAt(cam.transform.position); // ��Ŀ�� ī�޶� �ٶ󺸵��� ����

            // �и� ���� ���� ������Ʈ
            UpdateParryMarker(closestEnemy);
        }
    }

    public EnemyHealth FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + cam.transform.forward * frontOffset, detectionRadius, enemyLayer);
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        float halfFieldOfView = cam.fieldOfView * 0.5f;

        foreach (Collider col in colliders)
        {
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>(); // Collider ��� EnemyHealth ������Ʈ�� ������
            if (enemyHealth != null)
            {
                Transform enemyTransform = col.transform;
                Vector3 cameraToEnemy = enemyTransform.position - cam.transform.position;
                float angle = Vector3.Angle(cam.transform.forward, cameraToEnemy);

                if (angle <= halfFieldOfView)
                {
                    float distanceToEnemy = Vector3.Distance(cam.transform.position, enemyTransform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestEnemy = enemyHealth;
                        closestDistance = distanceToEnemy;
                    }
                }
            }
        }

        return closestEnemy;
    }

    void ActivateLockOn(EnemyHealth target)
    {
        isLockOn = true;
        marker.SetActive(true);

        originalCamera.gameObject.SetActive(false);
        virtualCamera.LookAt = target.transform;
        virtualCamera.gameObject.SetActive(true);
    }

    void DeactivateLockOn()
    {
        isLockOn = false;
        marker.SetActive(false);
        marker.transform.SetParent(null); // ��Ŀ�� �θ� ���� ����

        virtualCamera.LookAt = null;
        virtualCamera.gameObject.SetActive(false);
        originalCamera.gameObject.SetActive(true);
    }

    void UpdateParryMarker(EnemyHealth enemy)
    {
        if (enemy.IsParryGuageFull)
        {
            if (!parryMarker.activeSelf) parryMarker.SetActive(true);
        }
        else
        {
            if (parryMarker.activeSelf) parryMarker.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        // ĳ���� ��ġ���� ī�޶��� ���� �������� frontOffset �Ÿ���ŭ ������ ������ ����մϴ�.
        Vector3 sphereCenter = transform.position + cam.transform.forward * frontOffset;

        Gizmos.color = Color.red; // Gizmos ������ ���������� ����
                                  // ���� ��ġ�� ���Ǿ �׸��ϴ�.
        Gizmos.DrawWireSphere(sphereCenter, detectionRadius);
    }
}
