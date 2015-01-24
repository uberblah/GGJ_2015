using UnityEngine;
using System.Collections;

public class Player : Actor {
    public Camera view = null;

    public float force; // Force added on move
    public float cursorWeight = 0.25f; //weight of cursor in camera position

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

    protected override int GetAction()
    {
        for (int i = 1; i < 10; i++)
        {
            if (Input.GetAxisRaw("Action" + i) > 0.0f) return i;
        }
        return 0;
    }

    protected override bool GetUseTool()
    {
        return Input.GetAxisRaw("Tool") > 0.0f;
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

    protected override void Update()
    {
        base.Update();

        Vector3 mousePos = view.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newpos = transform.position + ((mousePos - view.transform.position) * cursorWeight);
        view.transform.position = new Vector3(newpos.x, newpos.y, 0.0f);
        //Debug.Log("World mouse pos:" + view.ScreenToWorldPoint(Input.mousePosition).x + "," + view.ScreenToWorldPoint(Input.mousePosition).y);
        //Debug.Log("View mouse pos:" + Input.mousePosition.x + "," + Input.mousePosition.y);

    }
}
