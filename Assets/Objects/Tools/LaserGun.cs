using UnityEngine;
using System.Collections;

public class LaserGun : Tool
{
    public GameObject   projectile; // Projectile object we use

    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        showMenu = false; // Temporary
    }

    public override void Activate()
    {
        if (projectile != null)
        {
            GameObject newProj = (GameObject)Instantiate(projectile, owner.transform.position, Quaternion.identity); // Shoot from parent, not where we are
            newProj.GetComponent<Projectile>().SetDirection(Vector2.right);
            // Play sound
            audio.Play();
        }
        else
        {
            Debug.Log("Projectile not set for laser gun");
        }
    }
}
