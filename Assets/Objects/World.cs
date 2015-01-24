using System;
using UnityEngine;
public class World
{
	private int xSize, ySize; //How big is the world?
	public string worldName; //Name of the planet
	public LandChunk[,] tiledLand; //This is the grid of land, X,Y cooridinates for that chunk of land.
	public void Start()
	{
		xSize = 50; //Fifty chunks of land across
		ySize = 50; //Fifty chunks of land high
		tiledLand = new LandChunk[xSize,ySize]; //Let's initalize this array of land!
		worldName = "Unknown World"; //You have a better name?
		GenerateRandWorld (); //Let there be light!!!
	}

	protected void GenerateRandWorld()
	{
		for (int x = 0; x < xSize; x++) //For Every land across
		{
			for (int y = 0; y<ySize; y++) //And every down
			{
				//Determine the height from Perlin Noise
				tiledLand[x,y].Height = Mathf.PerlinNoise(((float)x/(float)xSize),((float)y/(float)ySize));
				tiledLand[x,y].DetermineForm(); //And let the land determine what itself is based on it's height.
			}
		}
	}
}

