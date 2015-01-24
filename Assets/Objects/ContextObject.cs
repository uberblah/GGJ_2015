using UnityEngine;
using System.Collections;

public class ContextObject : MonoBehaviour
{
    public Camera cam;
    protected bool      showMenu;

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
            //Camera camera = GameObject.Find("Main Camera");
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
    }

    void OnMouseExit()
    {
        showMenu = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
