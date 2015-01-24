using UnityEngine;
using System.Collections;

public class enemy : Actor
{
    private GameObject player;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        forceMul = 10;
	}

    protected override Vector2 GetMove()
    {
        // Move towards player
        return Vector2.MoveTowards(transform.position, player.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();
	}
}
