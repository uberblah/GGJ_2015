using UnityEngine;
using System.Collections;

public class enemy : Actor
{
    protected enum EnemyState { Idle, Chase, Retreat };

    protected GameObject      player; // Reference to player
    protected EnemyState      state; // Current AI state
    protected float           lastSwitch; // Last time we switched states
    protected Vector2         moveVec; // Direction we want to move in

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        // Find first player
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        // Set force (public later?)
        forceMul = 10;
        // Set starting state
        state = EnemyState.Idle;
        lastSwitch = Time.time;
        moveVec = Vector2.zero;
	}

    protected override Vector2 GetMove()
    {
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
        // Change to chase after amount of time
        if (Time.time > lastSwitch + 4.0f)
            SwitchState(EnemyState.Chase);

        moveVec = Vector2.zero;
    }

    protected virtual void DoChase()
    {
        // Change to retreat after amount of time
        if(Time.time > lastSwitch + 4.0f)
            SwitchState(EnemyState.Retreat);
        // Move us towards player
        moveVec = player.transform.position - transform.position;
    }

    protected virtual void DoRetreat()
    {
        // Change to chase after amount of time
        if (Time.time > lastSwitch + 4.0f)
            SwitchState(EnemyState.Chase);
        // Move away from player
        moveVec = transform.position - player.transform.position;
    }
}
