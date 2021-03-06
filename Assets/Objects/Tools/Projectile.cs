﻿using UnityEngine;
using System.Collections;

public class Projectile : Actor
{
    private Vector2     moveDir; // Movement direction

    public void SetDirection(Vector2 dir) { moveDir = dir; }

    // Move in set direction
    protected override Vector2 GetMove()
    {
        return moveDir;
    } 

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        forceMul = 300;
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        // Damage object on touch
        Destructible objDes = col.gameObject.GetComponent<Destructible>();
        if (objDes != null)
        {
            objDes.Damage(20);
        }

        Destroy(gameObject);
    }
}
