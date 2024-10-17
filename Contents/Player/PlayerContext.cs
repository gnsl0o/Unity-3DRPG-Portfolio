using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerContext : MonoBehaviour
{
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationsData AnimationsData { get; private set; }

    public PlayerActionStateMachine actionStateMachine;

    public IAttackStrategy attackStrategy;

    public Playermovement Player;

    public InputBuffer inputBuffer;

    public IAttackStrategy GetAttackStrategy()
    {
        return attackStrategy;
    }

    public Interactions interaction;

    public PlayerInput Input;

    public Parry parry;

    public Animator anim;

    public Rigidbody Rigidbody;
    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        attackStrategy = GetComponent<SwordAttackStrategy>();
        Player = GetComponent<Playermovement>();
        Rigidbody = GetComponent<Rigidbody>();
        MainCameraTransform = Camera.main.transform;
        inputBuffer = GetComponent<InputBuffer>();

        parry = GetComponent<Parry>();

        AnimationsData.Initialize();

        // ��� �ʼ� ������Ʈ�� �ʱ�ȭ�� �Ŀ� ���� �ӽ��� ����
        actionStateMachine = new PlayerActionStateMachine(this);
        actionStateMachine.SetState(actionStateMachine.SheathedState);
    }

    public Animator Animator
    {
        get { return anim; }
    }

    private void Update()
    {
        actionStateMachine.Update();
        actionStateMachine.HandleInput();

        Debug.Log("���� ����: " + actionStateMachine.CurrentState.ToString());  
    }

    private void FixedUpdate()
    {
        actionStateMachine.PhysicsUpdate(); // ���� ���¿� ���� �Է� ó��
    }
}
