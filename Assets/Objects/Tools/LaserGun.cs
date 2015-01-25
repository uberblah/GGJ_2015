using UnityEngine;
using System.Collections;

public class LaserGun : Tool
{
    public GameObject   projectile; // Projectile object we use

    public override void Start()
    {
        base.Start();
        //showMenu = false; // Temporary
    }

    public override void Activate()
    {
        if (projectile != null)
        {
            // Create projectile
            Vector3 handPos = owner.transform.position + new Vector3(0,1,0);
            handPos.z = 0;
            GameObject newProj = (GameObject)Instantiate(projectile, handPos, Quaternion.identity); // Shoot from parent, not where we are
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
