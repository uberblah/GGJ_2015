using System;
using UnityEngine;

public class Stick : Tool
{
    public float stickLength; // How long is the stick?
public void Start ()
{
    toolName = "Stick";
    toolDesc = "This is your basic stick, it whacks things";
}

protected override void Activate ()
{
base.Activate ();
    Vector2 vec = this.body.GetVector;
    float roatation = this.body.rotation;
    body.AddForceAtPosition (new Vector2 (vec.x+(Math.Sin(rotatation)*stickLength), (Math.Cos(rotation)*stickLength)*vec.y));
}
}

