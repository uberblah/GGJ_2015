using UnityEngine;
using System.Collections;

public class enemy : Actor
{

	// Use this for initialization
	void Start ()
    {
        forceMul = 1.5f;
	}

    protected override Vector2 GetMove()
    {
        // Move towards player
        GameObject player = GameObject.Find("Player");
        return Vector2.MoveTowards(transform.position, player.transform.position, 10);
    }
	
	// Update is called once per frame
	void Update ()
    { 
	}
}
