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
        //TEMPORARY
        methods["Test1"] = this.DoTest;
        methods["Test2"] = this.DoTest;
        methods["Test3"] = this.DoTest;
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
    private void OnMouseOver()
    {
        showMenu = true;
        curr = this;
    }

    private void OnMouseExit()
    {
        showMenu = false;
        if (curr == this)
            curr = null;
    }

    public void DoMethod(Actor a, string method)
    {
        curr.methods[method](a);
    }

    public void DoTest(Actor a)
    {
        Debug.Log(a.name + " called test in " + name);
    }
}
