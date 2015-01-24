using System;
using UnityEngine;
public class Item : MonoBehaviour
{
	protected Rigidbody2D body; //Object Body
	public bool inInventory; // Is this thing in anybody's inventory?
	protected float value; // Assumed to be a random value to the object, either how much it contributes to a task or goal


	public void Start()
	{
		inInventory = false;
	}


}

