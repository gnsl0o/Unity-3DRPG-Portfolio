using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using static UnityEngine.Rendering.DebugUI;

public class CheckDistance : Conditional
{
    public SharedGameObject targetGameObject;
    public SharedBool isChasing;

    public float distanceToCheck = 2f;
    public float distanceToReturn = 10f;
    public float attackRange = 1f;

    // 플레이어와의 거리가 distanceToCheck보다 멀 경우 true 반환
    public override TaskStatus OnUpdate()
    {
        if (targetGameObject == null || targetGameObject.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, targetGameObject.Value.transform.position);

        if (distance > distanceToReturn)
        {
            Debug.Log("너무 멀어");
            isChasing.Value = false;
            return TaskStatus.Failure;
        }

        if (distance <= distanceToReturn && distance >= distanceToCheck)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}