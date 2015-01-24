using System;
using UnityEngine;

public class Stick : Tool
{
public float stickLength; // How long is the stick?
void Start ()
{
toolName = "Stick";
toolDesc = "This is your basic stick, it whacks things";
}

override void Activate ()
{
base.Activate ();
		body.AddForceAtPosition (new Vector2 (this.body.GetVector * this.body.transform.rotation * stickLength));
}
}

