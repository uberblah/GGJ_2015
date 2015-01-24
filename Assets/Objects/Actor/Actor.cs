using UnityEngine;
using System.Collections;

//TODO: SWITCH BULLETS TO USE ENERGY INSTEAD OF SOME MULTIPLIER
//TODO: GO TO DESTRUCTIBLE, MAKE IT SUBTRACT THE ARMOR
//TODO: MOVE WEAPON INFORMATION AND CALLS TO A WEAPONCONTROLLER CLASS
//TODO: FIGURE OUT HOW TO REPRESENT A PLAYER WITH DIFFERENT WEAPONS

public class Actor : MonoBehaviour
{
    protected Rigidbody2D           body;
    protected CircleCollider2D      coll;
    protected LineRenderer          lnmkr;
    protected float                 forceMul;

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

    protected virtual bool GetUseTool()
    {
        return false;
    }

    //returns the index of the contexted method
    protected virtual int GetAction()
    {
        return 0;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null) Debug.Log(name + " failed to find its body!");
        coll = GetComponent<CircleCollider2D>();
        if (coll == null) Debug.Log(name + " failed to find its collider!");
        lnmkr = GetComponent<LineRenderer>();
        if (lnmkr == null) Debug.Log(name + " failed to fine its line renderer!");
    }

    protected virtual void FixedUpdate()
    {
        body.AddForce(forceMul * getMove().normalized);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 diff = getTarget() - body.position;
        body.rotation = (Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x)) - 90.0f;
    }
}
