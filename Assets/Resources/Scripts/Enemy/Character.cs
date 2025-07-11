using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Character : Entity
{
    [Header("References")]
    public GameObject checkPoint;
    public Entity positionOpponent;
    public Transform opponentTranform;
    public HealthBarUI healthBar;

    // ---Model---
    public CharacterModel characterModel => (CharacterModel)model;

    [Header("Attack Control")]
    public bool isTriggerAttackEnemy = false;
    public bool isDelayAttack = false;
    public bool isContinueAttack = false;
    public bool isAttackToIdle = false;
    public bool canAttackContinuously = true;
    public bool isKnockOut = false;
    public bool hasCamera = false;
    public float timerAttack = 0f;

    [Header("State Timing")]
    public bool isWaitToMove = false;
    public float stateSwitchDelay = 0.2f;
    public float timeSinceLastSwitch = 0f;

    [Header("Visual")]
    public bool isFlip = false;

    override protected void Awake()
    {
        base.Awake();
        
    }
    public void CreateEnemy(float health, float damage, float attackCoolDown, float drag, float timeToMove, float timeToIdle, bool canAttackContiously, HealthBarUI healthBar, float timeAttackContinue)
    {
        RotateTowardsTarget();
        isContinueAttack = true;
        isIdle = true;
        isAlive = true;
        isAttack = false;
        isInjured = false;
        wasHit = false;
        isTriggerAttackEnemy = false;
        isDelayAttack = false;
        isAttackToIdle = false;
        isWaitToMove = false;
        capsuleCollider.isTrigger = false;
        isKnockOut = false;
        this.canAttackContinuously = canAttackContiously;
        rb.drag = drag;
        model = new CharacterModel(this, health, damage, attackCoolDown, timeToMove, timeToIdle, timeAttackContinue);
        this.healthBar = healthBar;
        healthBar.SetupCamera(transform, tagObject);
    }
    public void RotateTowardsTarget()
    {
        if(tagObject == EntityType.Player)
        {
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            transform.Rotate(0f, 0f, 0f);
        }
    }
    public void SetupOpponent(Entity opponent)
    {
        positionOpponent = opponent;
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

        if (model.health <= 0f)
        {
            characterModel.stateMachine.ChangeState(characterModel.knockOutState);
        }
        
        if (!isAttackCoolingDown)
        {
            isAttack = CheckForEntityInRange(whatIsLayer,tagObject.ToString());
        }

        if (isAttackCoolingDown)
        {
            model.attackTimer += Time.deltaTime;
            if (model.attackTimer >= model.attackCooldown)
            {
                isAttackCoolingDown = false;
                model.attackTimer = 0f;
            }
        }
        characterModel.stateMachine.currentState.Update();


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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * model.rotationSpeed);
    }

    public void AttackTriggerEvent()
    {
        isTriggerAttackEnemy = true;
    }
    public void ApplyDamageOnHit()
    {
        didHitTarget = true;
    }
    public void OnHit()
    {
        wasHit = true;
    }
    public override void TakeDamage(float damageAmount, float varInjury,Transform opponent)
    {
        base.TakeDamage(damageAmount, varInjury, opponent);
        healthBar.UpdateHealthBar(damageAmount*1.0f / model.maxHealth);
        this.opponentTranform = opponent;
        isFlip = true;
    }
    public void KnockOutTrigger()
    {
        isKnockOut = true;
    }
}
