using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContextObject : MonoBehaviour
{
    public delegate void ContextMethod(Actor a);

    public Camera                   cam;
    protected static ContextObject  curr;
    protected bool                  showMenu;

    public Dictionary<string, ContextMethod> methods;

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
            // Build list 
            string list = "";
            int i = 1;
            foreach(string name in methods.Keys)
            {
                list += i + ". " + name + "\n";
                i++;
            }
            // Show box
            GUI.Box(new Rect(0,0,100,150),list);
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

    static void DoMethod(Actor a, int method)
    {
        ContextObject applyTo = curr;
    }
}
