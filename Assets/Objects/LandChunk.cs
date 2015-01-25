using System;
using UnityEngine;
public class LandChunk : MonoBehaviour
{
    public float Height;
    public enum landTypes {water, notWater};
    public landTypes landType;
	public Sprite spiteSprite;
	public Sprite Land, NotLand;
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
        if (Height >= .25f)
        {
            landType = landTypes.notWater;
        }
        else
        {
            landType = landTypes.water;
        }

        if (landType == landTypes.water)
        {
            assignSprite(NotLand);
        }
        else
        {
            assignSprite(Land);
        }
    }
}
