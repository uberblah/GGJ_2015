using UnityEngine;
using System.Collections;

public class LaserGun : Tool
{
    public GameObject projectile; // Projectile object we use

    public override void Start()
    {
        base.Start();
    }

    public override void Activate()
    {
        if (projectile != null)
        {
            Instantiate(projectile, this.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(Vector2.right);
        }
    }
}
