using System;
using UnityEngine;
public class LandChunk
{
	public float Height;
	public enum landTypes {water, notWater};
	public landTypes landType;

	public void DetermineForm()
	{
		if (Height >= .1) 
		{
			landType = landTypes.notWater;
		} 
		else 
		{
			landType = landTypes.water;
		}
	}

}

