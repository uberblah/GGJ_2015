using UnityEngine;
using System.Collections;

public class enemy : Actor
{
    protected enum EnemyState { Idle, Chase };

    protected GameObject      player; // Reference to player
    protected EnemyState      state; // Current AI state
    protected float           lastSwitch; // Last time we switched states

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
	}

    protected override Vector2 GetMove()
    {
        // Decide how to move based on what state we're in
        switch (state)
        {
            case EnemyState.Idle:
                return Vector2.zero;
            case EnemyState.Chase:
                // Move towards player
                return player.transform.position - transform.position;
            default:
                return Vector2.zero;
        }
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
            default:
                DoIdle();
                break;
        }
	}

    protected virtual void DoIdle()
    {
        // Change to chase after amount of time
        if (Time.time > lastSwitch + 4.0f)
            state = EnemyState.Chase;
    }

    protected virtual void DoChase()
    {

    }
}
