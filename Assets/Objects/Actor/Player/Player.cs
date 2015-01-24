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

        Vector3 mousePos = view.ScreenToWorldPoint(Input.mousePosition);
        const float plyWeight = 2; // Weight of player on the camera focus
        view.transform.position = new Vector3(
            ((transform.position.x / plyWeight) + mousePos.x / 2), ((transform.position.y / plyWeight) + mousePos.y / 2), 0);

        Debug.Log("World mouse pos:" + view.ScreenToWorldPoint(Input.mousePosition).x + "," + view.ScreenToWorldPoint(Input.mousePosition).y);
        Debug.Log("View mouse pos:" + Input.mousePosition.x + "," + Input.mousePosition.y);

    }
}
