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
            // Create projectile
            GameObject newProj = (GameObject)Instantiate(projectile, owner.transform.position, Quaternion.identity); // Shoot from parent, not where we are
            // Ignore collision with player
            Physics2D.IgnoreCollision(newProj.collider2D, owner.collider2D);
            // Shoot in in direction of mouse pointer
            newProj.GetComponent<Projectile>().SetDirection(
                Camera.main.ScreenToWorldPoint(Input.mousePosition) - owner.transform.position);
            // Play sound
            audio.Play();
        }
        else
        {
            Debug.Log("Projectile not set for laser gun");
        }
    }
}
