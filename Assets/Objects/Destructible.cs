using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float health = 100.0f;
    public float armor = 0.5f;
    private Actor act;

    void Start()
    {
        act = GetComponent<Actor>();
    }

    public void Damage(float amount)
    {
        health -= amount * armor;
        if (health <= 0.0f) OnDestroy();

        // Call actor OnDamage, if applicable
        if (act != null)
        {
            act.OnDamage();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;

        // Call actor OnHeal, if applicable
        if (act != null)
        {
            act.OnHeal();
        }
    }

    //this can be overridden to create other behaviors for destruction
    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
