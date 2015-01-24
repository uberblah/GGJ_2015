using System;
using System.Collections.Generic;
using UnityEngine;
public class Item : ContextObject
{
    protected static List<Item> dropped = new List<Item>();

	protected Rigidbody2D   body; //Object Body
	protected float         value; // Assumed to be a random value to the object, either how much it contributes to a task or goal
	public bool             beingUsed; // Is it being used punk?

	#region Owner Stuff
	protected Actor owner; // Who owns the tool?
	
	// Does this tool have an owner?
	public bool IsOwned()
	{
		return owner != null;
	}
	// Give this tool an owner
	protected virtual void SetOwner(Actor myNewOwner)
	{
		owner = myNewOwner;
	}
	// Make this tool lack an owner
	protected virtual void RemoveOwner(Actor ownerWhoDoesntWantMe)
	{
		owner = null;
	}
	#endregion

	public virtual void Start()
	{
		beingUsed = false;
        dropped.Add(this);
	}

    public virtual void Update()
    {
        // Take us out of the world if we have an owner
        if (IsOwned())
        {
            if(GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().isTrigger = true; // Disable collisions
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0); // Make invisible
        }
        // Put us back in the world
        if (IsOwned())
        {
            if (GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().isTrigger = false; // Enable collisions
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255); // Make visible
        }
    }

	public void SetItemValue(float v)
	{
		value = v;
	}

	public void ItemPickedUp()
	{
		//Is the item put into an inventory?
		renderer.enabled = false;
		body.collider2D.enabled = false;
        dropped.Remove(this);
	}

	public void ItemDrop(Vector2 WhereWasIPutDown)
	{
		renderer.enabled = true;
		body.collider2D.enabled = true;
		body.position = WhereWasIPutDown;
		beingUsed = false;
        if(!dropped.Contains(this)) dropped.Add(this);
	}

	public void ItemNowBeingUsed()
	{
		beingUsed = true;
		renderer.enabled = true;
		body.collider2D.enabled = true;
	}

    public static Item FindNearestDropped(Vector2 pos)
    {
        float mindiff = float.MaxValue;
        Item nearest = null;
        foreach (Item i in dropped)
        {
            float dist = (new Vector2(i.transform.position.x, i.transform.position.y) - pos).magnitude;
            if (dist < mindiff || nearest == null)
            {
                mindiff = dist;
                nearest = i;
            }
        }
        return nearest;
    }
}

