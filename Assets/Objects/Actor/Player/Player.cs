using UnityEngine;
using System.Collections;

public class Player : Actor {
    public Camera view = null;

    public float force; // Force added on move
    public float cursorWeight = 0.25f; //weight of cursor in camera position

    // Temporary
    public GameObject   projectile;
    static float        lastFire;

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
        if (Input.GetKeyDown(KeyCode.Alpha1)) return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) return 9;
        return 0;
    }
    //Inventory::Pick Up Item
    protected override bool GetPickup()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
    //Inventory::Drop Current Item
    protected override bool GetDrop()
    {
        return Input.GetKeyDown(KeyCode.G);
    }
    //Inventory::Next Item
    protected override bool GetNextItem()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    //Inventory::Prev Item
    protected override bool GetPrevItem()
    {
        return Input.GetKeyDown(KeyCode.Q);
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

        // TEMPORARY: fire projectile
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            if (Time.time > lastFire + 1.0f)
            {
                Instantiate(projectile,this.transform.position,Quaternion.identity);
                projectile.GetComponent<Projectile>().SetDirection(Vector2.right);
                lastFire = Time.time;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }
}
