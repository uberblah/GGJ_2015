using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    public Camera view = null;

    // the sprite's orientation
    // this is in terms of stage direction (http://quizlet.com/48871336/stage-areas-body-positions-casting-and-auditioning-summative-assessment-terms-and-concepts-flash-cards/)
    // left and right are relative to the actor
    // Open means face visible, closed means face hidden
    // (the actor's left and right, not the camera's)
    // directions are enumerated clockwise order
    public enum SpriteOrientation
    {
        FullFront,
        QuarterOpenRight,   // also called quarter right
        ProfileRight,
        QuarterClosedRight, // also called three quarter right
        FullBack,
        QuarterClosedLeft,  // also called three quarter left
        ProfileLeft,
        QuarterOpenLeft     // also quarter left
    };

    protected Rigidbody2D           body;
    protected Collider2D            coll;
    protected LineRenderer          lnmkr;
    protected float                 forceMul;
    protected float                 rotation;
    protected Inventory             inv;
    public    ContextObject         selected = null;
    protected    SpriteOrientation     orientation;

    protected float lastToolUse; // Last time we used tool

    public SpriteOrientation GetSpriteOrientation()
    {
        return orientation;
    }

    protected virtual Vector2 GetMove()
    {
        return new Vector2(0.0f, 0.0f);
    }

    protected virtual Vector2 GetTarget()
    {
        return body.position + new Vector2(0.0f, 1.0f);
    }

    protected virtual int GetContextMethod()
    {
        return 0;
    }

    //Inventory::Use Current Tool
    protected virtual bool GetUseTool()
    {
        return false;
    }
    //Inventory::Pick Up Item
    protected virtual bool GetPickup()
    {
        return false;
    }
    //Inventory::Drop Current Item
    protected virtual bool GetDrop()
    {
        return false;
    }
    //Inventory::Next Item
    protected virtual bool GetNextItem()
    {
        return false;
    }
    //Inventory::Prev Item
    protected virtual bool GetPrevItem()
    {
        return false;
    }

    //Inventory: Give us item
    public virtual void GiveItem(Item i)
    {
        inv.PickUp(i);
    }

    //returns the index of the contexted method to call
    protected virtual int GetAction()
    {
        return 0;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        // set the camera
        if (view == null)
        {
            view = GetComponent<Camera>();
            if (view == null) Debug.Log(name + " failed to find its camera");
        }

        // initialize the sprite orientation to profile right
        orientation = SpriteOrientation.ProfileLeft;

        inv = new Inventory();
        inv.Start();
        body = GetComponent<Rigidbody2D>();
        if (body == null) Debug.Log(name + " failed to find its body!");
        coll = GetComponent<Collider2D>();
        if (coll == null) Debug.Log(name + " failed to find its collider!");
    }

    protected virtual void FixedUpdate()
    {
        body.AddForce(forceMul * GetMove().normalized);
    }

    public virtual void OnDamage()
    {

    }

    public virtual void OnHeal()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 diff = GetTarget() - body.position;
        rotation = (Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x)) - 90.0f;
        Tool tool = inv.GetActive() as Tool;
        if (GetUseTool() && tool != null && Time.time > lastToolUse + tool.GetDelay())
        {
            tool.Activate();
            lastToolUse = Time.time;
        }
        int action = GetAction();
        if (action > 0 && selected != null) selected.DoMethod(this, action);
        if (GetDrop()) inv.PutDown(inv.GetActive(), this.transform.position);
        //TODO: FIND NEAREST OBJECT
        if (GetPickup()) inv.PickUp(Item.FindNearestDropped(transform.position));
        // figure out sprite orientation
        // the character is always facing the direction of movement
        if (body.velocity != Vector2.zero)
        {
            if (body.velocity.y == 0)
            {
                // sprite is profile to us
                if (body.velocity.x > 0)
                    orientation = SpriteOrientation.ProfileLeft;
                else
                    orientation = SpriteOrientation.ProfileRight;
            }
            else if (body.velocity.y > 0)
            {
                // sprite is facing away from us
                if (body.velocity.x == 0)
                    orientation = SpriteOrientation.FullBack;
                else if (body.velocity.x > 0)
                    orientation = SpriteOrientation.QuarterClosedLeft;
                else
                    orientation = SpriteOrientation.QuarterClosedRight;
            }
            else
            {
                // sprite is facing toward us
                if (body.velocity.x == 0)
                    orientation = SpriteOrientation.FullFront;
                else if (body.velocity.x > 0)
                    orientation = SpriteOrientation.QuarterOpenLeft;
                else
                    orientation = SpriteOrientation.QuarterOpenRight;
            }
        }
    }
}
