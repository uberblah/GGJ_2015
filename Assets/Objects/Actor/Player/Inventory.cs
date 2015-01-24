using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Inventory
{
	List<Item> items;
	int currentlyActiveItem = 0;
	bool usingItem;
	public void Start()
	{
		usingItem = false;
		items = new List<Item> ();
	}

    public void rightShift()
    {
        currentlyActiveItem = (currentlyActiveItem + 1) % items.Count;
    }

    public void leftShift()
    {
        currentlyActiveItem--;
        if (currentlyActiveItem <= 0) currentlyActiveItem = items.Count - 1;
    }

    public Item getActive()
    {
        if (currentlyActiveItem < items.Count)
            return items[currentlyActiveItem];
        else return null;
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

