using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // 플레이어가 시야에 감지되었음을 확인하는 이벤트
    public event Action<bool, GameObject> OnVisibilityChanged;

    // 시야 영역의 반지름과 시야 각도
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    // 마스크 2종
    public LayerMask targetMask, obstacleMask;

    // Target mask에 ray hit된 transform을 보관하는 리스트
    public List<Transform> visibleTargets = new List<Transform>();

    // 임시
    private GameObject player;

    private void Start()
    {
        // 0.2초 간격으로 코루틴 호출
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
        // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);


        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // 플레이어와 forward와 target이 이루는 각이 설정한 각도 내라면
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                if(!Physics.Raycast(transform.position, dirToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        // previousVisible 변수가 비어있지 않다면 OnVisibilityChanged 이벤트 호출
        if(previousVisible != (visibleTargets.Count > 0))
        {
            if (visibleTargets.Count > 0)
            {
                player = visibleTargets[0].gameObject; // 예를 들어 첫 번째 대상을 선택합니다.
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
        Vector3 forward = transform.forward; // 캐릭터의 전방 방향
        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up); // 왼쪽 경계선 방향
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2, Vector3.up); // 오른쪽 경계선 방향
        Vector3 leftRayDirection = leftRayRotation * forward; // 왼쪽 경계선 벡터
        Vector3 rightRayDirection = rightRayRotation * forward; // 오른쪽 경계선 벡터

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftRayDirection * viewRadius); // 왼쪽 경계선 그리기
        Gizmos.DrawRay(transform.position, rightRayDirection * viewRadius); // 오른쪽 경계선 그리기
    }
}
