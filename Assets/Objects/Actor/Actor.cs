using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
    protected Rigidbody2D           body;
    protected CircleCollider2D      coll;
    protected LineRenderer          lnmkr;
    protected float                 forceMul;
    protected float                 rotation;
    protected Tool                  tool = null;
    public    ContextObject         selected = null;

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
    }

    protected virtual void FixedUpdate()
    {
        body.AddForce(forceMul * GetMove().normalized);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 diff = GetTarget() - body.position;
        rotation = (Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x)) - 90.0f;
        if (GetUseTool() && tool != null) tool.Activate();
        int action = GetAction();
        if (action > 0 && selected != null) selected.DoMethod(this, action);
    }
}
