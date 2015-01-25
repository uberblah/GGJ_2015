using System;
using UnityEngine;

public class World : MonoBehaviour
{
private int xSize, ySize; //How big is the world?
public string worldName; //Name of the planet
public LandChunk[,] tiledLand; //This is the grid of land, X,Y cooridinates for that chunk of land.
private System.Random rand;
public GameObject Tile;
public GameObject WateryTile;

public void Start ()
{
    renderer.enabled = true;
    xSize = 200; //Fifty chunks of land across
    ySize = 200; //Fifty chunks of land high
    tiledLand = new LandChunk[xSize, ySize]; //Let's initalize this array of land!
    worldName = "Unknown World"; //You have a better name?
    rand = new System.Random ();
    GenerateRandWorld (); //Let there be light!!
}
protected void GenerateRandWorld ()
    {
		GameObject TiTi;
    for (int x = 0; x < xSize; x++) { //For Every land across
    for (int y = 0; y<ySize; y++) { //And every down
    //Determine the height from Perlin Noise //And let the land determine what itself is based on it's height.
	float heiHei = Mathf.PerlinNoise ((float)Math.Abs (x / ((float)xSize) + (rand.NextDouble () - rand.NextDouble())*.75f), (float)Math.Abs (y / ((float)ySize) + (rand.NextDouble () - rand.NextDouble())*.75f));
	if (heiHei >= .25)
{
	TiTi = (GameObject) Instantiate(Tile);
	TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)(x), (float)(y)));
	TiTi.GetComponent<LandChunk>().Height = heiHei;
	TiTi.GetComponent<LandChunk>().DetermineForm();
}
				else
{
	TiTi = (GameObject) Instantiate(WateryTile);
	TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)(x), (float)(y)));
	TiTi.GetComponent<LandChunk>().Height = heiHei;
	TiTi.GetComponent<LandChunk>().DetermineForm();
}
    }
    }
	
}
	/*
    protected void OnGUI()
    {
    for (int x = 0; x < xSize; x++) //For Every land across
    {
    for (int y = 0; y<ySize; y++) //And every down
    {
    if (tiledLand[x,y].landType == LandChunk.landTypes.water)
    {
	//GUI.DrawTexture(new Rect ((float)(x*10), (float)(y*10), 10f, 10f), NotLand);
    GUI.Label (new Rect ((float)x*15f, (float)y*15f, 15f, 15f), "W");
    }
    else
    {
	//GUI.DrawTexture(new Rect ((float)(x*10), (float)(y*10), 10f, 10f), Land);
    GUI.Label (new Rect ((float)x*15f, (float)y*15f, 15f, 15f), "L");
    }
    }
    }
    }*/
}

