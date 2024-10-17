using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Playermovement : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field:Header("Collisions")]
    [field: SerializeField] public CapsuleColliderUtility ColliderUtility {  get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationsData AnimationsData { get; private set; }


    public Rigidbody Rigidbody { get; private set; }

    public Animator Animator { get; private set; }

    public PlayerInput Input {  get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public PlayerContext playerContext;

    public PlayerMovementStateMachine movementStateMachine;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        Input = GetComponent<PlayerInput>();

        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
        AnimationsData.Initialize();

        MainCameraTransform = Camera.main.transform;

        playerContext = GetComponent<PlayerContext>();

        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void OnValidate()
    {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }

    void Start()
    {
        movementStateMachine.SetState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}
