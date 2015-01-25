﻿using UnityEngine;
using System.Collections;

public class Player : Actor
{
    public float        force; // Force added on move
    public float        cursorWeight = 0.25f; //weight of cursor in camera position

    // Sprite stuff
    public Sprite[]     standingSprites; // Standing sprites
    private Vector3     initialScale; // Scale we started with

    // Footsteps
    private float       lastStep; // Time of last footstep sound
    public AudioClip    leftFootstep;
    public AudioClip    rightFootstep;

    // Hurt
    private bool        hurt; // Are we hurtin'?
    public float        hurtTime; // How long we flash red
    public float        hurtForce; // Force of a hurt
    private float       lastHurt; // Last time we were hurt (for red flash)

    protected override Vector2 GetMove()
    {
        if (GetUseTool())
            return Vector2.zero; // Kill momentum when we use a tool

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
        return Input.GetAxisRaw("Fire1") > 0.0f;
    }

    protected override void Start()
    {
        base.Start();
        // Set initial values
        forceMul = force;
        hurt = false;
        initialScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 mousePos = view.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newpos = transform.position + ((mousePos - view.transform.position) * cursorWeight);
        view.transform.position = new Vector3(newpos.x, newpos.y, 0.0f);
        //Debug.Log("World mouse pos:" + view.ScreenToWorldPoint(Input.mousePosition).x + "," + view.ScreenToWorldPoint(Input.mousePosition).y);
        //Debug.Log("View mouse pos:" + Input.mousePosition.x + "," + Input.mousePosition.y);

        // Cycle items
        if (GetNextItem())
        {
            inv.RightShift();
        }
        if (GetPrevItem())
        {
            inv.LeftShift();
        }

        // Change animation
        Animator anim = GetComponent<Animator>();
        if (GetMove() != Vector2.zero)
        {
            if (GetMove().x < 0)
            {
                anim.CrossFade("Walk_Sprite", 0f);
                Vector3 theScale = initialScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                orientation = SpriteOrientation.ProfileLeft;
            }
            else if(GetMove().x > 0)
            {
                anim.CrossFade("Walk_Sprite", 0f);
                transform.localScale = initialScale;
                orientation = SpriteOrientation.ProfileRight;
            }
            else if (GetMove().y < 0)
            {
                anim.CrossFade("Walk_Front", 0f);
                transform.localScale = initialScale;
                orientation = SpriteOrientation.FullBack;
            }
            else if (GetMove().y > 0)
            {
                anim.CrossFade("Back_Walk", 0f);
                transform.localScale = initialScale;
                orientation = SpriteOrientation.FullFront;
            }
        }
        else
        {
            // Standing based on last orientation
            anim.CrossFade("Standing", 0f);

            switch (orientation)
            {
                case SpriteOrientation.FullFront:
                    GetComponent<SpriteRenderer>().sprite = standingSprites[1];
                    break;
                case SpriteOrientation.FullBack:
                    GetComponent<SpriteRenderer>().sprite = standingSprites[2];
                    break;
                default:
                    GetComponent<SpriteRenderer>().sprite = standingSprites[0];
                    break;
            }
        }

        GetComponent<SpriteRenderer>().sprite = standingSprites[2];

        // Flash red if hurt
        if (hurt)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            // Check if we need to end hurt condition
            if (Time.time > lastHurt + hurtTime)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                hurt = false;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Play footstep noise
        if (GetMove() != Vector2.zero && Time.time > lastStep + 0.2f)
        {
            AudioSource aSource = GetComponent<AudioSource>();
            if (Random.Range(0,3) == 0)
                aSource.PlayOneShot(leftFootstep);
            else
                aSource.PlayOneShot(rightFootstep);

            lastStep = Time.time;
        }
    }

    public void OnGUI()
    {
        // Show active object, temporary?
        if(inv.GetActive() != null)
            GUI.Box(new Rect(0, 0, 200, 20), "Active item: " + inv.GetActive().gameObject.name);

        // Show health
        
        GUI.Box(new Rect(0,Screen.height - 20, GetComponent<Destructible>().health * 2, 20), "Health");

		// Show Score
		GUI.Box (new Rect (Screen.width - 100, Screen.height - 20, 100, 20), "Score: " + inv.totalScore.ToString()); 
    }

    void OnCollisionEnter2D(Collision2D col)
    {

    }

    public override void OnDamage()
    {
        hurt = true;
        lastHurt = Time.time;
        // Force away from where we're facing
        switch(orientation)
        {
            case SpriteOrientation.ProfileLeft:
                body.AddForce(Vector2.right * hurtForce);
                break;
            case SpriteOrientation.ProfileRight:
                body.AddForce(Vector2.right * -hurtForce);
                break;
            case SpriteOrientation.FullBack:
                body.AddForce(Vector2.up * hurtForce);
                break;
            case SpriteOrientation.FullFront:
                body.AddForce(Vector2.up * -hurtForce);
                break;
        }
        
    }
}
