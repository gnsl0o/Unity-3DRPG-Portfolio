using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LockOn : MonoBehaviour
{
    public GameObject marker; // 락온 마커
    private GameObject currentMarker; // 현재 활성화된 마커 오브젝트
    public GameObject parryMarker;

    public CinemachineVirtualCamera originalCamera;
    public CinemachineVirtualCamera virtualCamera;
    public Camera cam;
    public bool isLockOn = false;
    public LayerMask enemyLayer;

    public Transform playerTransform;

    public float detectionRadius = 10f;
    public float frontOffset = 5f;

    private EnemyHealth closestEnemy; // 락온 대상을 저장하는 변수

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
                    Debug.Log("발견된 적 = " +  closestEnemy.name);
                    ActivateLockOn(closestEnemy);
                    transform.LookAt(closestEnemy.transform.position);
                }
            }
        }

        if (isLockOn && closestEnemy != null)
        {
            Transform markerPosition = closestEnemy.marker;
            marker.transform.position = markerPosition.position;

            marker.transform.LookAt(cam.transform.position); // 마커가 카메라를 바라보도록 설정

            // 패리 가능 상태 업데이트
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
            EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>(); // Collider 대신 EnemyHealth 컴포넌트를 가져옴
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
        marker.transform.SetParent(null); // 마커의 부모 연결 해제

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
        // 캐릭터 위치에서 카메라의 전방 방향으로 frontOffset 거리만큼 떨어진 지점을 계산합니다.
        Vector3 sphereCenter = transform.position + cam.transform.forward * frontOffset;

        Gizmos.color = Color.red; // Gizmos 색상을 빨간색으로 설정
                                  // 계산된 위치에 스피어를 그립니다.
        Gizmos.DrawWireSphere(sphereCenter, detectionRadius);
    }
}
