using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsEnemyVisible : Conditional
{
    public FieldOfView fieldOfView;

    public override TaskStatus OnUpdate()
    {
        // visibleTargets ����Ʈ�� ���� �ϳ��� ������ ������ ��ȯ
        if (fieldOfView.visibleTargets.Count > 0)
        {
            return TaskStatus.Success;
        }

        // �ƴϸ� ���и� ��ȯ
        return TaskStatus.Failure;
    }
}