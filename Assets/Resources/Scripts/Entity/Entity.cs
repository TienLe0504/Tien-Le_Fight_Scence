using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum EntityType
{
    Player,
    Enemy
}
public class Entity : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;

    [Header("Player Data")]
    public Model model;
    [SerializeField] public EntityType tagObject;
    [SerializeField] public LayerMask whatIsLayer;

    [Header("Attack Settings")]
    [SerializeField] public Transform attackPoint;
    [SerializeField] public float attackRadius = 1f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] public float attackDistance = 2f;

    [Header("Player States")]
    public bool isIdle = false;
    public bool isAttack = false;
    public bool isAttackCoolingDown = false;
    public bool didHitTarget = false;
    public bool wasHit = false;
    public bool isInjured = false;
    public bool isAlive = true;
    public bool isWinner = false;

    public bool isAttacking { get; private set; }

    protected virtual void Start(){}
    protected virtual void FixedUpdate(){ }
    protected virtual void Update(){

        if (!isAlive)
        {
            return;
        }
    }
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        isAlive = true;
    }

    public virtual void ApplyMovementForce(Vector3 velocity, Vector3 moveDirection)
    {

        rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        Vector3 rayOrigin = attackPoint.position;
        Vector3 rayDirection = transform.forward; 

        Gizmos.color = Color.cyan; 
        Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * attackDistance);
    }
    public void PerformMeleeAttack(LayerMask whatIsLayer, string tag)
    {

        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, whatIsLayer);


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag))
            {
                hitCollider.GetComponent<Entity>().TakeDamage(model.damage, model.attackComboCount,transform);
            }
        }
    }
    public virtual void TakeDamage(float damageAmount, float varInjury, Transform opponent)
    {
        isInjured = true;
        model.injuryComboCount = varInjury;
        model.DamageDame(damageAmount);
    }

    public bool CheckForEntityInRange(LayerMask whatIsLayer, string tag)
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, whatIsLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tag))
            {
                Entity entity = hitCollider.GetComponent<Entity>();
                if (entity != null && entity.isAlive)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void WinGameTrigger()
    {
        isWinner = true;
    }
}
