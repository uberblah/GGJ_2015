using UnityEngine;
using System.Collections;

public class Hausteroids : MonoBehaviour
{
    public GameObject asteroid;
    public Hausenship ship;
    public float rate = 0.1f;
    public float acceleration = 0.01f;
    public float minSpeed = 1.5f;
    public float maxSpeed = 3.0f;
    public float distance = 20.0f;
    public float lifeSpan = 60.0f;
    float last = 0.0f;

    void Update()
    {
        if (ship.rigidbody.useGravity)
        {
            if (Time.time - last >= rate)
            {
                last = Time.time;
                Vector3 pos = ship.transform.position + new Vector3(distance, Random.Range(ship.floor, ship.ceil), 0.0f);
                GameObject go = Instantiate(asteroid, pos, new Quaternion()) as GameObject;
                go.GetComponent<Rigidbody>().velocity = new Vector3(-Random.Range(minSpeed, maxSpeed), 0.0f, 0.0f);
                Destroy(go, lifeSpan);
            }
            rate -= acceleration * Time.deltaTime;
        }
    }
}
