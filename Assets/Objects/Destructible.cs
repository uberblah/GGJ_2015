using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float health = 100.0f;
    public float armor = 0.5f;

    public void Damage(float amount)
    {
        health -= amount * armor;
        if (health <= 0.0f) OnDestroy();
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }

    protected virtual void OnDamage(float amount){}
    protected virtual void OnHeal(float amount){}

    //this can be overridden to create other behaviors for destruction
    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
