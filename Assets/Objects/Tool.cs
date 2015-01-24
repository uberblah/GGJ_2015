using System;
using UnityEngine;

/*
 * This is a generic tool class, different tools accomplish different things.
 * It can only be used if it's owned. If it isn't owned, it's just an object.
 */

public class Tool
{
    String toolName; //What's the tool called?
    String toolDesc; //Short description of the tool

    Rigidbody2D body; //The tool has to have a body!
	
    #region Owner Stuff
    Actor owner; // Who owns the tool?

    // Does this tool have an owner?
    bool isOwned()
    {
    return owner != null;
    }
    // Give this tool an owner
    virtual void giveOwner(Actor myNewOwner)
    {
    owner = myNewOwner;
    }
    // Make this tool lack an owner
    virtual void loseOwner(Actor ownerWhoDoesntWantMe)
    {
    owner = null;
    }
    #endregion

    virtual void Activate()
    {
    //This is the activate function, its use differs on the tool. A stick pokes, a torch burns.
    }

}

