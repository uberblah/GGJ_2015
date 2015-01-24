using System;
using UnityEngine;
public class LandChunk
{
	public float Height;
	public enum landTypes {water, notWater};
	public landTypes landType;
	public LandChunk()
	{
		Height = 0.0f;
		landType = landTypes.water;
	}

	public void DetermineForm()
	{
		if (Height >= .25) 
		{
			landType = landTypes.notWater;
		} 
		else 
		{
			landType = landTypes.water;
		}
	}

}

