using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsEnemyVisible : Conditional
{
    public FieldOfView fieldOfView;

    public override TaskStatus OnUpdate()
    {
        // visibleTargets 리스트에 적이 하나라도 있으면 성공을 반환
        if (fieldOfView.visibleTargets.Count > 0)
        {
            return TaskStatus.Success;
        }

        // 아니면 실패를 반환
        return TaskStatus.Failure;
    }
}