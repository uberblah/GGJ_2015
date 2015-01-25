using System;
using UnityEngine;

public class World : MonoBehaviour
{
    private int segmentGenDepth; //How many segments do we make per direction at a time?
    private float coarseSeed; // determines and saves the broad character of the map
    private int segmentSize; // tiles per map generation segment
    private int mapSegments; // number of segments in the global map
    public string worldName; //Name of the planet
    public float[,] tiledLand; //This is the entire grid of land chunks
    public bool[,] generatedSegment; // keep track of what has already been generated
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
	public GameObject Tree; //Not the data structure, the brown barky thing that isn't a doggy.
	public GameObject Enemy; //...Exactly what it sounds like chicklet.
    //public GameObject ship; // set in Unity
    public Tool weapon; // set in Unity
    public GameObject ship;
    public float waterCutoff;
	public float baseGroundCutoff;
	public float grassyCutoff;
	public float forestyCutoff;
    public Player player;

    public void Start()
    {
        renderer.enabled = true;
        waterCutoff = .20f;
		baseGroundCutoff = .4f;
		grassyCutoff = .6f;
		forestyCutoff = .8f;

        segmentSize = 30;
        mapSegments = 100;
        segmentGenDepth = 2;
        int mapTiles = mapSegments * segmentSize;
        tiledLand = new float[mapTiles, mapTiles]; //Let's initalize this array of land!
        generatedSegment = new bool[mapSegments, mapSegments];
        worldName = "Unknown World"; //You have a better name?
        rand = new System.Random();
        coarseSeed = (float)rand.NextDouble() * 400;
        GenerateRandWorld(new Vector2(mapTiles/2,mapTiles/2)); //Let there be light!!
        // pick a dry spot for the player
        Vector2 initialPlayerPos;
        do
        {
            initialPlayerPos = new Vector2(
                ((float)rand.NextDouble() - 1) * segmentGenDepth * 2 + mapTiles / 2,
                ((float)rand.NextDouble() - 1) * segmentGenDepth * 2 + mapTiles / 2);
            Debug.Log("(" + initialPlayerPos.x + "," + initialPlayerPos.y + ")");
        } while (!isItPassable(initialPlayerPos));
        player.transform.position = initialPlayerPos;
        // put ship and gun near player spawn
        weapon.transform.position = initialPlayerPos + new Vector2(5, 2);
        ship.transform.position = initialPlayerPos + new Vector2(7, -21);
    }

    // check if it's possible to stand on a bit of land
    public bool isItPassable(Vector2 locale)
    {
        return (tiledLand[(int)locale.x, (int)locale.y] >= waterCutoff);
    }

    // origin will be rounded to nearest segmentGenDepth
    // radius is the distance around the origin that will be generated
    public void GenerateRandWorld(Vector2 origin)
    {
		GameObject Adversary;
        // round origin to the nearest segment boundary
        origin.x = (int)((origin.x + segmentSize / 2) / segmentSize) * segmentSize;
        origin.y = (int)((origin.y + segmentSize / 2) / segmentSize) * segmentSize;
        //
		GameObject partyPart; //PARTS ENTER INTO THE WORLD!!! VIVE LE PARTS, THEY FLEE THE OPPRESIVE PRISON WHICH WAS ONCE THEIRS!
        GameObject TiTi;
		GameObject Boulder; //His name is boulder, he's my pet rock.
		GameObject Leafy; //Her name is leafy, she's not a rock or my pet, but she's food for my turtle.
        // first, create noise for coarse generation
        float coarseVariability = 6;
        float fineVariability = 3;
        // create the offsets array
        int coarseSize = segmentGenDepth * 2;
        float[,] heiOffset = new float[coarseSize + 1, coarseSize + 1]; // create array for caorse offset storage
        for (int indX = 0; indX <= coarseSize; indX++)
        { // for every coarse unit across
            int coarseX = (int)origin.x / segmentSize - segmentGenDepth + indX;
            for (int indY = 0; indY <= coarseSize; indY++)
            { // for every coarse unit down
                int coarseY = (int)origin.y / segmentSize - segmentGenDepth + indY;
                // Use Perlin noise to get a height offset
                float noiseValue = Mathf.PerlinNoise(coarseSeed + coarseVariability * (float)coarseX / (float)coarseSize, coarseSeed + coarseVariability * (float)coarseY / (float)coarseSize); // create noise
                heiOffset[indX, indY] = noiseValue * .75f; // scale down
            }
        }
        for (int indX = 0; indX < coarseSize; indX++)
        { // for every coarse unit across
            int coarseX = (int)origin.x / segmentSize - segmentGenDepth + indX;
            for (int indY = 0; indY < coarseSize; indY++)
            { // for every coarse unit down
                int coarseY = (int)origin.y / segmentSize - segmentGenDepth + indY;
                // have we already generated this?
                if (!generatedSegment[coarseX, coarseY])
                {
                    generatedSegment[coarseX, coarseY] = true;
                    // get surrounding boundary offsets
                    float q11 = heiOffset[indX, indY];
                    float q12 = heiOffset[indX, indY + 1];
                    float q21 = heiOffset[indX + 1, indY];
                    float q22 = heiOffset[indX + 1, indY + 1];
                    float fineSeed = (float)rand.NextDouble() * 400;
                    for (int fineX = 0; fineX < segmentSize; fineX++)
                    { //For Every land across
                        float r1 = q11 * (segmentSize - fineX) / segmentSize + q21 * fineX / segmentSize;
                        float r2 = q12 * (segmentSize - fineX) / segmentSize + q22 * fineX / segmentSize;
                        for (int fineY = 0; fineY < segmentSize; fineY++)
                        { //And every down
                            float thisOffset = r1 * (segmentSize - fineY) / segmentSize + r2 * fineY / segmentSize;
                            // where are we?
                            int xCoord = coarseX * segmentSize + fineX;
                            int yCoord = coarseY * segmentSize + fineY;
                            //Determine the height from Perlin Noise //And let the land determine what itself is based on it's height.
                            //float noiseValue = Mathf.PerlinNoise(fineSeed + (float)xCoord / (float)generationUnitSize, fineSeed + (float)yCoord / (float)generationUnitSize); // create noise
                            float noiseValue = Mathf.PerlinNoise(fineSeed + fineVariability * (float)fineX / (float)segmentSize, fineSeed + fineVariability * (float)fineY / (float)segmentSize); // create noise
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
                                if (rand.NextDouble() >= (double).999)
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
                                else if (rand.NextDouble() >= (double).9999)
                                {
                                    Adversary = (GameObject)Instantiate(Enemy);
                                    Adversary.GetComponent<enemy>().transform.position = new Vector3((float)xCoord, (float)yCoord);
                                }
                            }
                            else if (heiHei < forestyCutoff)
                            {
                                TiTi = (GameObject)Instantiate(ForestyTile);
                                TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
                                TiTi.GetComponent<LandChunk>().Height = heiHei;
                                TiTi.GetComponent<LandChunk>().DetermineForm();
                                if (rand.NextDouble() >= (double).999)
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
                                else if (rand.NextDouble() >= (double).999)
                                {
                                    Adversary = (GameObject)Instantiate(Enemy);
                                    Adversary.GetComponent<enemy>().transform.position = new Vector3((float)xCoord, (float)yCoord);
                                }
                            }
                            else
                            {
                                TiTi = (GameObject)Instantiate(RockyTile);
                                TiTi.GetComponent<LandChunk>().setLocation(new Vector2((float)xCoord, (float)yCoord));
                                TiTi.GetComponent<LandChunk>().Height = heiHei;
                                TiTi.GetComponent<LandChunk>().DetermineForm();
                                if (rand.NextDouble() >= (double).99)
                                {
                                    partyPart = (GameObject)Instantiate(rarePart);
                                    partyPart.GetComponent<Item>().value = 100;
                                    partyPart.GetComponent<Item>().transform.position = new Vector2((float)xCoord, (float)yCoord);
                                    Adversary = (GameObject)Instantiate(Enemy);
                                    Adversary.GetComponent<enemy>().transform.position = new Vector3((float)xCoord, (float)yCoord);
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
}