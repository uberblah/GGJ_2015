using System;
using UnityEngine;

/*
 * This is a generic tool class, different tools accomplish different things.
 * It can only be used if it's owned. If it isn't owned, it's just an object.
 */

public class Tool : Item
{
    public String toolName; //What's the tool called?
    public String toolDesc; //Short description of the tool
	


    public virtual void Activate()
    {
    //This is the activate function, its use differs on the tool. A stick pokes, a torch burns.
    }

}

