using System;
using UnityEngine;
public class LandChunk : MonoBehaviour
{
    public float Height;
	public Sprite spiteSprite;
	public Sprite Water, Dirt, Grass, Forest, Rock;
	public Vector2 location;

    public void Start()
    {

    }

	public void assignSprite(Sprite s)
	{
		spiteSprite = s;
	}
	public void setLocation(Vector2 l)
	{
		location = l;
		transform.position = location;
		transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z+5.0f);

	}

    public void DetermineForm()
    {
		if (Height < .2f) 
		{
			assignSprite(Water);
		}
		else if (Height < .4f) 
		{
			assignSprite(Dirt);
		}
		else if (Height < .6f) 
		{
			assignSprite(Grass);
		}
		else if (Height < .8f) 
		{
			assignSprite(Forest);
		}
		else 
		{
			assignSprite(Rock);
		}
    }
}
