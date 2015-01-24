using System;
using UnityEngine;
public class Item : MonoBehaviour
{
	protected Rigidbody2D body; //Object Body
	protected float value; // Assumed to be a random value to the object, either how much it contributes to a task or goal
	public bool beingUsed; // Is it being used punk?

	#region Owner Stuff
	protected Actor owner; // Who owns the tool?
	
	// Does this tool have an owner?
	public bool isOwned()
	{
		return owner != null;
	}
	// Give this tool an owner
	protected virtual void giveOwner(Actor myNewOwner)
	{
		owner = myNewOwner;
	}
	// Make this tool lack an owner
	protected virtual void loseOwner(Actor ownerWhoDoesntWantMe)
	{
		owner = null;
	}
	#endregion

	public void Start()
	{
		beingUsed = false;
	}

	public void setItemValue(float v)
	{
		value = v;
	}

	public void itemPickedUp()
	{
		//Is the item put into an inventory?
		renderer.enabled = false;
		body.collider2D.enabled = false;
	}

	public void itemDrop(Vector2 WhereWasIPutDown)
	{
		renderer.enabled = true;
		body.collider2D.enabled = true;
		body.position = WhereWasIPutDown;
		beingUsed = false;
	}

	public void itemNowBeingUsed()
	{
		beingUsed = true;
		renderer.enabled = true;
		body.collider2D.enabled = true;
	}
}

