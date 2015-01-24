using System;
using UnityEngine;
public class LandChunk
{
	public float Height;
	enum landTypes {water, notWater};
	landTypes landType;

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

