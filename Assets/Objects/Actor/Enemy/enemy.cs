﻿using UnityEngine;
using System.Collections;

public class enemy : Actor
{
    public float              damage; // Amount of damage we'll do
    public float              force; // Amount of force while moving

    protected enum EnemyState { Idle, Chase, Retreat };

    protected GameObject      player; // Reference to player
    protected EnemyState      state; // Current AI state
    protected float           lastSwitch; // Last time we switched states
    protected Vector2         moveVec; // Direction we want to move in
    protected Animator        anim; // Animation
    private Vector3           initialScale; // Scale we started with

    // Audio
    public AudioClip          wakeUp;
    public AudioClip          hurt;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        // Find first player
        player = GameObject.FindWithTag("Player");
        // Roll dice for super-fast enemy
        if (Random.Range(1, 20) == 1)
        {
            forceMul = 105;
        }
        else
        {
            // Set force with random component
            forceMul = force * Random.Range(0.8f, 1.4f);
        }
        // Set starting state
        state = EnemyState.Idle;
        lastSwitch = Time.time;
        moveVec = Vector2.zero;
        anim = GetComponent<Animator>();
        anim.CrossFade("Idle", 0f);
        initialScale = transform.localScale;
	}

    protected override Vector2 GetMove()
    {
        // Flip sprite
        if (moveVec.x > 0) // right
        {
            Vector3 theScale = initialScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else // left
        {
            transform.localScale = initialScale;
        }
        // Send movement direction
        return moveVec;
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();

        // Run AI states
        switch (state)
        {
            case EnemyState.Idle:
                DoIdle();
                break;
            case EnemyState.Chase:
                DoChase();
                break;
            case EnemyState.Retreat:
                DoRetreat();
                break;
            default:
                DoIdle();
                break;
        }
	}

    // Change to another state
    // Recommend usage as it also sets switch time
    protected void SwitchState(EnemyState newState)
    {
        state = newState;
        lastSwitch = Time.time;
    }

    protected virtual void DoIdle()
    {
        // Chase player if he gets close
        if (Vector2.Distance(transform.position, player.transform.position) < 15)
        {
            SwitchState(EnemyState.Chase);
            // play wake up sound
            GetComponent<AudioSource>().PlayOneShot(wakeUp);
        }
        // Do not move
        moveVec = Vector2.zero;
    }

    protected virtual void DoChase()
    {
        // Change to retreat after amount of time
        //if(Time.time > lastSwitch + 4.0f)
        //    SwitchState(EnemyState.Retreat);
        // Move us towards player
        anim.CrossFade("Wolfy_Run", 0f);
        moveVec = player.transform.position - transform.position;

        // Retreat if player is dead
        if (player.GetComponent<Destructible>().GetDead())
            SwitchState(EnemyState.Retreat);
    }

    protected virtual void DoRetreat()
    {
        // Change to chase after amount of time
        if (Time.time > lastSwitch + 1.2f)
            SwitchState(EnemyState.Chase);
        // Move away from player
        moveVec = transform.position - player.transform.position + new Vector3(Random.Range(-10,10),Random.Range(-10,10),0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Move back when we bump into stuff
        //if (col.gameObject != player &&
        if(col.gameObject.GetComponent<Projectile>() == null)
        {
            SwitchState(EnemyState.Retreat);
        }

        // Hurt player
        if (col.gameObject == player)
        {
            Destructible plyDes = col.gameObject.GetComponent<Destructible>();
            if (plyDes != null)
            {
                plyDes.Damage(damage);
            }
        }
    }

    public override void OnDamage()
    {
        base.OnDamage();
        // Play hurt clip
        GetComponent<AudioSource>().PlayOneShot(hurt);
        // Remove
        if (GetComponent<Destructible>().GetDead())
            Destroy(gameObject);
    }
}
