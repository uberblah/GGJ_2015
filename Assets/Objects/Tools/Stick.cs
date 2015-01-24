using System;
using UnityEngine;

public class Stick : Tool
{
public float stickLength; // How long is the stick?
public override void Start ()
{
    base.Start();
    toolName = "Stick";
    toolDesc = "This is your basic stick, it whacks things";
}

public void setStickLength(float newLength)
{
stickLength = newLength;
}

public override void Activate ()
{
base.Activate ();
    Vector2 vec = this.body.position;
    double rotat = (double)this.body.rotation;
		body.AddForceAtPosition (new Vector2 (vec.x+(float)(Math.Cos(rotat)*stickLength*2), (float)(Math.Sin(rotat)*stickLength*2)+vec.y), 
		                                     new Vector2 (vec.x+(float)(Math.Cos(rotat)*stickLength), (float)(Math.Sin(rotat)*stickLength)+vec.y));
}
}