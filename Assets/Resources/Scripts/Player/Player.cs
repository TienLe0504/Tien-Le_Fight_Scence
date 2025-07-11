using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : Entity
{
    [Header("Input")]
    [SerializeField] private FloatingJoystick joystick;
     //---Model---
    public PlayerModel playerModel => (PlayerModel)model;

    [Header("Player States")]
    public bool isJumping = false;
    public bool isGrounded = false;
    public bool hasTriggeredEvent = false;
    public bool isKnockOut = false;

    public void InitializeInput(FloatingJoystick joystick)
    {
        this.joystick = joystick;
    }
    override protected void Awake()
    {
        base.Awake();

    }
    public void InitializePlayer(float health, float damage, Vector3 pos)
    {
        RotateTowardsTarget();
        isIdle = true;
        isAlive = true;
        isAttack = false;
        isInjured = false;
        wasHit = false;
        hasTriggeredEvent = false;
        didHitTarget = false;
        isJumping = false;
        isGrounded = false;
        isAttackCoolingDown = false;
        isWinner = false;
        isKnockOut = false;
        capsuleCollider.isTrigger = false;
        if (anim != null)
        {
            foreach (AnimatorControllerParameter parameter in anim.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    anim.SetBool(parameter.name, false);
                }
            }
        }
        model = new PlayerModel(this, health,damage);
        playerModel.attackComboCount = 0f;
        transform.position = pos;
    }
    public void RotateTowardsTarget()
    {
        transform.Rotate(0f, 0f, 0f);
    }
    override protected void Start()
    {
        base.Start();
    }
    override protected void Update()
    {
        base.Update();
        if (model == null || !GameManger.Instance.isPlayerGame)
            return;
        if (model != null)
        {
            if (model.health <= 0f)
            {
                playerModel.stateMachine.ChangeState(playerModel.knockOutState);
            }
        }
        bool wasIdle = isIdle;
        model.moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        isIdle = (model.moveDirection == Vector3.zero);
        isAttack = CheckForEntityInRange(whatIsLayer,tagObject.ToString());
        playerModel.stateMachine.currentState.Update();
    }
    private void OnEnable()
    {
        ListenerManager.Instance.AddListener<Vector2>(EventType.Jump, Jump);
    }
    private void OnDisable()
    {
        if (ListenerManager.HasInstance || ListenerManager.Instance != null)
        {
            ListenerManager.Instance.RemoveListener<Vector2>(EventType.Jump, Jump);
        }
    }
    public override void ApplyMovementForce(Vector3 velocity, Vector3 moveDirection)
    {
        base.ApplyMovementForce(velocity, moveDirection);
        Flip(moveDirection);
    }
    public void Flip(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime *model.rotationSpeed);
    }
    public void Jump(Vector2 jumpVector)
    {
        if (isJumping) return;
        jumpVector.Normalize();
        if (jumpVector.y < 0) return; 
        isJumping = true;
        isGrounded = true;

    }
    
    public void AttackTriggerEvent()
    {
        hasTriggeredEvent = true;
    }
    public void ApplyDamageOnHit()
    {
        didHitTarget = true;
    }
    public void OnHit()
    {
        wasHit = true;
    }
    public override void TakeDamage(float damageAmount, float varInjury, Transform opponent)
    {
        base.TakeDamage(damageAmount, varInjury, opponent);
        GameManger.Instance.TakeDamagePlayerUI(damageAmount);
    }
    public void KnockOutTrigger()
    {
        isKnockOut = true;
    }
}
