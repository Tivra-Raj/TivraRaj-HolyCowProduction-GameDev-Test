using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    [SerializeField]
    protected int MaxHP;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected bool isPlayer = false;
    [SerializeField]
    protected Weapon weapon;
    [SerializeField]
    protected Transform shootpoint;

    protected float currentHP;

    public virtual void Start()
    {
        currentHP = MaxHP;
    }
    public virtual void ApplyDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
