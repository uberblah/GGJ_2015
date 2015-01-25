using System;
using UnityEngine;

public class World : MonoBehaviour
{
    private int generationUnitSize; //How much world do we make at a time?
    // save variables for generating map pieces on the fly
    private float coarseSeed; // determines broad character of the map
    private int fineSize; // size of each map piece
    public string worldName; //Name of the planet
    public float[,] tiledLand; //This is the grid of land, X,Y cooridinates for that chunk of land.
    private System.Random rand;
    public GameObject Tile;
    public GameObject WateryTile;
	public GameObject GrassyTile;
	public GameObject ForestyTile;
	public GameObject RockyTile;
	public GameObject commonPart;
	public GameObject uncommonPart;
	public GameObject rarePart;
	public GameObject Rock; //not the music, the round ground seed thing
	public GameObject Tree; //Not the data structure, 
    public float waterCutoff;
	public float baseGroundCutoff;
	public float grassyCutoff;
	public float forestyCutoff;
    public Player player;

    public void Start()
    {
        renderer.enabled = true;
        generationUnitSize = 250; // chunks of land to generate at a time
        waterCutoff = .20f;
		baseGroundCutoff = .4f;
		grassyCutoff = .6f;
		forestyCutoff = .8f;
        tiledLand = new float[generationUnitSize, generationUnitSize]; //Let's initalize this array of land!
        worldName = "Unknown World"; //You have a better name?
        rand = new System.Random();
        coarseSeed = (float)rand.NextDouble() * 400;
        fineSize = 30;
        GenerateRandWorld(Vector2.zero,8); //Let there be light!!
        // pick a dry spot for the player
        Vector2 initialPlayerPos;
        do
        {
            initialPlayerPos = new Vector2(
                (float)rand.NextDouble() * generationUnitSize,
                (float)rand.NextDouble() * generationUnitSize);
        } while (!isItPassable(initialPlayerPos));
    }
    public bool isItPassable(Vector2 locale)
    {
        return (tiledLand[(int)locale.x / generationUnitSize, (int)locale.y / generationUnitSize] >= waterCutoff);
    }

    // origin will be rounded to nearest fineSize
    // radius is the distance around the origin that will be generated
    protected void GenerateRandWorld(Vector2 origin,int radius)
    {
		GameObject partyPart; //PARTS ENTER INTO THE WORLD!!! VIVE LE PARTS, THEY FLEE THE OPPRESIVE PRISON WHICH WAS ONCE THEIRS!
        GameObject TiTi;
		GameObject Boulder; //His name is boulder, he's my pet rock.
		GameObject Leafy; //Her name is leafy, she's not a rock or my pet, but she's food for my turtle.
        // first, create noise for coarse generation
        int coarseSize = generationUnitSize / fineSize;
        float coarseVariability = 6;
        float fineVariability = 3;
        // create the offsets array
        float[,] heiOffset = new float[coarseSize + 1, coarseSize + 1]; // create array for caorse offset storage
        for (int coarseX = 0; coarseX <= coarseSize; coarseX++)
        { // for every coarse unit across
            for (int coarseY = 0; coarseY <= coarseSize; coarseY++)
            { // for every coarse unit down
                // Use Perlin noise to get a height offset
                float noiseValue = Mathf.PerlinNoise(coarseSeed + coarseVariability * (float)coarseX / (float)coarseSize, coarseSeed + coarseVariability * (float)coarseY / (float)coarseSize); // create noise
                heiOffset[coarseX, coarseY] = noiseValue * .75f; // scale down
            }
        }
        for (int coarseX = 0; coarseX < coarseSize; coarseX++)
        { // for every coarse unit across
            for (int coarseY = 0; coarseY < coarseSize; coarseY++)
            { // for every coarse unit down
                float q11 = heiOffset[coarseX, coarseY];
                float q12 = heiOffset[coarseX, coarseY + 1];
                float q21 = heiOffset[coarseX + 1, coarseY];
                float q22 = heiOffset[coarseX + 1, coarseY + 1];
                float fineSeed = (float)rand.NextDouble() * 400;
                for (int fineX = 0; fineX < fineSize; fineX++)
                { //For Every land across
                    float r1 = q11 * (fineSize - fineX) / fineSize + q21 * fineX / fineSize;
                    float r2 = q12 * (fineSize - fineX) / fineSize + q22 * fineX / fineSize;
                    for (int fineY = 0; fineY < fineSize; fineY++)
                    { //And every down
                        float thisOffset = r1 * (fineSize - fineY) / fineSize + r2 * fineY / fineSize;
                        // where are we?
                        int xCoord = coarseX * fineSize + fineX;
                        int yCoord = coarseY * fineSize + fineY;
                        //Determine the height from Perlin Noise //And let the land determine what itself is based on it's height.
                        //float noiseValue = Mathf.PerlinNoise(fineSeed + (float)xCoord / (float)generationUnitSize, fineSeed + (float)yCoord / (float)generationUnitSize); // create noise
                        float noiseValue = Mathf.PerlinNoise(fineSeed + fineVariability * (float)fineX / (float)fineSize, fineSeed + fineVariability * (float)fineY / (float)fineSize); // create noise
                        noiseValue = noiseValue * .25f; // at most .25 different
                        float heiHei = thisOffset + noiseValue;
                        tiledLand[xCoord, yCoord] = heiHei;
                        if (heiHei < waterCutoff)
                        {
							TiTi = (GameObject)Instantiate(WateryTile);
                            TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
                            TiTi.GetComponent<LandChunk>().Height = heiHei;
                            TiTi.GetComponent<LandChunk>().DetermineForm();
                        }
                        else if (heiHei < baseGroundCutoff)
                        {
							TiTi = (GameObject)Instantiate(Tile);
                            TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
                            TiTi.GetComponent<LandChunk>().Height = heiHei;
                            TiTi.GetComponent<LandChunk>().DetermineForm();
							if (rand.NextDouble() >= (double).999)
							{
								 partyPart = (GameObject)Instantiate(uncommonPart);
								partyPart.GetComponent<Item>().value = 50;
								partyPart.GetComponent<Item>().transform.position = new Vector2((float)xCoord, (float)yCoord);
							}
							else if (rand.NextDouble() >= (double).999)
							{
								Boulder = (GameObject)Instantiate(Rock);
								Boulder.GetComponent<MeshFilter>().transform.position = new Vector3((float)xCoord, (float)yCoord);
							}
                        }
						else if (heiHei < grassyCutoff)
						{
							TiTi = (GameObject)Instantiate(GrassyTile);
							TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
							TiTi.GetComponent<LandChunk>().Height = heiHei;
							TiTi.GetComponent<LandChunk>().DetermineForm();
							if (rand.NextDouble() >=(double).999)
							{
								partyPart = (GameObject)Instantiate(commonPart);
								partyPart.GetComponent<Item>().value = 25;
								partyPart.GetComponent<Item>().transform.position = new Vector2((float)xCoord, (float)yCoord);

							}
							else if (rand.NextDouble() >= (double).999)
							{
								Boulder = (GameObject)Instantiate(Rock);
								Boulder.GetComponent<MeshFilter>().transform.position = new Vector3((float)xCoord, (float)yCoord);
							}
							else if (rand.NextDouble() >= (double).999)
							{
								Leafy = (GameObject)Instantiate(Tree);
								Leafy.GetComponent<MeshFilter>().transform.position = new Vector3((float)xCoord, (float)yCoord);
							}
						}
						else if (heiHei < forestyCutoff)
						{
							TiTi = (GameObject)Instantiate(ForestyTile);
							TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
							TiTi.GetComponent<LandChunk>().Height = heiHei;
							TiTi.GetComponent<LandChunk>().DetermineForm();
							if (rand.NextDouble() >=(double).999)
							{
								partyPart = (GameObject)Instantiate(rarePart);
								partyPart.GetComponent<Item>().value = 100;
								partyPart.GetComponent<Item>().transform.position = new Vector2((float)xCoord, (float)yCoord);
							}
							else if (rand.NextDouble() >= (double).999)
							{
								Leafy = (GameObject)Instantiate(Tree);
								Leafy.GetComponent<MeshFilter>().transform.position = new Vector3((float)xCoord, (float)yCoord);
							}
						}
						else
						{
							TiTi = (GameObject)Instantiate(RockyTile);
							TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
							TiTi.GetComponent<LandChunk>().Height = heiHei;
							TiTi.GetComponent<LandChunk>().DetermineForm();
							if (rand.NextDouble() >=(double).99)
							{
							partyPart = (GameObject)Instantiate(rarePart);
							partyPart.GetComponent<Item>().value = 100;
							partyPart.GetComponent<Item>().transform.position = new Vector2((float)xCoord, (float)yCoord);
							}
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

    }
}