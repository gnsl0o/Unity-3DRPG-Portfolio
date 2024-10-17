using Unity.VisualScripting;
using UnityEngine;
public interface IAttackStrategy
{
    public void Attack();

    public void EnableInput();

    // 애니메이션 이벤트로 호출될 메서드
    public void DisableInput();

    public void ResetAttackCount();
}