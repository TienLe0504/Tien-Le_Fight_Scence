using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model 
{
    public float speed = 1f;
    public float rotationSpeed = 15f;
    public Vector3 moveDirection;
    public float attackComboCount = 0;
    public float attackCooldown = 1.5f;
    public float attackTimer = 0f;
    public float injuryComboCount = 0f;
    public float health = 100f;
    public float damage = 10f;
    public float attackComboWindow;
    public float maxHealth;
    public float stepAtatck = 0.5f;
    public void IncreaseAttackCount(float maxHitValue)
    {
        attackComboCount += stepAtatck;
        if (attackComboCount > maxHitValue)  
        {
            attackComboCount = 0f; 
        }
    }
    public void DamageDame(float dame)
    {
        health-= dame;
    }
}
