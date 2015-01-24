using UnityEngine;
using System.Collections;

public class ContextObject : MonoBehaviour
{
    public Camera                   cam;
    protected static ContextObject  curr;
    protected bool                  showMenu;

    public string[]                 methods;

	// Use this for initialization
	void Start ()
    {
        showMenu = false;
	}

    // Show menu
    void OnGUI()
    {
        if (showMenu)
        {
            Vector2 guiPos = cam.WorldToScreenPoint (this.transform.position);
            GUI.BeginGroup(new Rect(guiPos.x, guiPos.y, 100, 150));
            GUI.Box(new Rect(0,0,100,150),"1. Test\n2.Test\n3.Test");
            GUI.EndGroup();
        }
    }

    // Show menu on mouse over
    void OnMouseOver()
    {
        showMenu = true;
        curr = this;
    }

    void OnMouseExit()
    {
        showMenu = false;
        if (curr == this)
            curr = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    static void DoMethod(Player p, int method)
    {
        ContextObject applyTo = curr;
    }
}
