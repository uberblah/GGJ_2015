using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Inventory
{
	List<Item> items;
	int currentlyActiveItem;
	bool usingItem;
	public void Start()
	{
		usingItem = false;
		List<Item> items = new List<Item> ();
	}

	public void itemIsNowBeingUsed(Item i)
	{
		currentlyActiveItem = items.IndexOf(i);
		usingItem = true;
		i.itemNowBeingUsed ();
	}

	public void pickUp(Item i)
	{
		items.Add (i);
		i.itemPickedUp ();
	}

	public void putDown(Item i, Vector2 LocationPutDown)
	{
		items.Remove (i);
		i.itemDrop (LocationPutDown);
	}
}

