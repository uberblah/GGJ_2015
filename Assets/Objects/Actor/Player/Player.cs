using UnityEngine;
using System.Collections;

public class Player : Actor
{
    public float        force; // Force added on move
    public float        cursorWeight = 0.25f; //weight of cursor in camera position

    // Sprite stuff
    private Vector3     initialScale; // Scale we started with
    private Animator    anim; // Animation

    // Audio
    private AudioSource aSource;
    private float       lastStep; // Time of last footstep sound
    public AudioClip    leftFootstep;
    public AudioClip    rightFootstep;
    public AudioClip    pickup;

    // Hurt
    private bool        hurt; // Are we hurtin'?
    public float        hurtTime; // How long we flash red
    public float        hurtForce; // Force of a hurt
    private float       lastHurt; // Last time we were hurt (for red flash)

    public Texture2D    crosshair; // Custom crosshair

    // For map generation
    public World        theWorld;

    protected override Vector2 GetMove()
    {
        // Don't move if dead
        if (GetComponent<Destructible>().GetDead())
            return Vector2.zero;

        if (GetUseTool())
        {
            return Vector2.zero; // Kill momentum when we use a tool
        }

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
        anim = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();
        // Hide mouse cursor
        Screen.showCursor = false;
    }

    protected override void Update()
    {
        base.Update();

        // Don't do any of this if we're dead
        if (GetComponent<Destructible>().GetDead())
        {
            Screen.showCursor = true;
            return;
        }

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
        if (GetMove() != Vector2.zero)
        {
            if (GetMove().x < 0)
            {
                anim.CrossFade("Walk_Sprite", 0f);
                transform.localScale = FlipScale();
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

            if (GetUseTool())
            {
                // Drop the tooltip
                MoveTooltip tt = GetComponentInChildren<MoveTooltip>();
                if (tt != null) Destroy(tt);
                // Shoot animation
                anim.CrossFade("Shoot", 0f);
                // Face towards mouse cursor
                if (mousePos.x < transform.position.x)
                {
                    transform.localScale = FlipScale();
                }
                else
                {
                    transform.localScale = initialScale;
                }
            }
        }

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

        // check if we need to generate more map segments
        int xSeg = (int)transform.position.x / theWorld.segmentSize;
        int ySeg = (int)transform.position.y / theWorld.segmentSize;
        if (!theWorld.generatedSegment[xSeg, ySeg] || !theWorld.generatedSegment[xSeg + 1, ySeg] ||
            !theWorld.generatedSegment[xSeg - 1, ySeg] || !theWorld.generatedSegment[xSeg, ySeg + 1] ||
            !theWorld.generatedSegment[xSeg, ySeg - 1] || !theWorld.generatedSegment[xSeg + 1, ySeg + 1] ||
            !theWorld.generatedSegment[xSeg - 1, ySeg - 1])
            theWorld.GenerateRandWorld(transform.position);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Play footstep noise
        if (GetMove() != Vector2.zero && Time.time > lastStep + 0.2f)
        {
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

		// Show Score
		GUI.Box (new Rect (Screen.width - 100, Screen.height - 20, 100, 20), "Score: " + inv.totalScore.ToString());

        // Pieces found
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        GUI.Box(new Rect(Screen.width/2 - 150, 0, 150, 30), "Pieces Found: " + inv.piecesFound, style);

        // Show health
        style.normal.textColor = Color.red;
        GUI.Box(new Rect(0, Screen.height - 30, 200, 30), "+" + GetComponent<Destructible>().health, style);

        // Reset button if dead
        if (GetComponent<Destructible>().GetDead())
        {
            if (GUI.Button(new Rect((Screen.width / 2) - 50, Screen.height - 50, 50, 50), "Restart"))
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
        else
        {
            // Show crosshair
            int cursorSizeX = 64;
            int cursorSizeY = 64;
            GUI.DrawTexture(new Rect(Input.mousePosition.x - cursorSizeX, (Screen.height - Input.mousePosition.y) - cursorSizeY, cursorSizeX, cursorSizeY), crosshair);
        }
    }

    public override void GiveItem(Item i)
    {
        base.GiveItem(i);
        // Play pickup sound
        aSource.PlayOneShot(pickup);
        // Give a small bit of health
        GetComponent<Destructible>().Heal(Random.Range(5, 7));
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

        // Die
        if (GetComponent<Destructible>().GetDead())
        {
            GetComponent<SpriteRenderer>().enabled = false;
            // Show game over text
            foreach (Transform child in view.transform)
            {
                if (child.name == "Game Over")
                    child.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    private Vector3 FlipScale()
    {
        Vector3 theScale = initialScale;
        theScale.x *= -1;
        return theScale;
    }
}
