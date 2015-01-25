using System.Collections.Generic;
using System.Collections;
using UnityEngine;
/*
 * Inventory of items
 */

public class Inventory
{
	List<Item>  items;
	int         currentlyActiveItem = 0;

	public void Start()
	{
		items = new List<Item> ();
	}

    public void RightShift()
    {
        currentlyActiveItem = (currentlyActiveItem + 1);
        if (currentlyActiveItem >= items.Count)
            currentlyActiveItem = 0;
    }

    public void LeftShift()
    {
        currentlyActiveItem--;
        if (currentlyActiveItem < 0)
            currentlyActiveItem = items.Count - 1;
    }

    public Item GetActive()
    {
        if (currentlyActiveItem < items.Count)
            return items[currentlyActiveItem];
        else return null;
    }

	public void ItemIsNowBeingUsed(Item i)
	{
		currentlyActiveItem = items.IndexOf(i);
		i.ItemNowBeingUsed ();
	}

	public void PickUp(Item i)
	{
		items.Add (i);
		i.ItemPickedUp ();
	}

	public void PutDown(Item i, Vector2 LocationPutDown)
	{
		items.Remove (i);
		i.ItemDrop (LocationPutDown);
	}
}

