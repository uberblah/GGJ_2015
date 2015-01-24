using UnityEngine;
using System.Collections;

public class Player : Actor {
    public Camera view = null;

    public float force; // Force added on move

    protected override Vector2 GetMove()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    protected override Vector2 GetTarget()
    {
        Vector2 mpos = Input.mousePosition;
        Vector3 mpos3 = view.ScreenToWorldPoint(new Vector3(mpos.x, mpos.y, 0.0f));
        return new Vector2(mpos3.x, mpos3.y);
    }

    protected override void Start()
    {
        base.Start();
        if (view == null)
        {
            view = GetComponent<Camera>();
            if (view == null) Debug.Log(name + " failed to find its camera");
        }

        forceMul = force;
    }

    void Update()
    {
        base.Update();
    }
}
