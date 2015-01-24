using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContextObject : MonoBehaviour
{
    public delegate void ContextMethod(Actor a);

    public Camera                   cam;
    protected static ContextObject  curr;
    protected bool                  showMenu;

    public List<KeyValuePair<string, ContextMethod>> methods;

	// Use this for initialization
	void Start ()
    {
        showMenu = false;
        methods = new List<KeyValuePair<string, ContextMethod>>();
        //TEMPORARY
        methods.Add(new KeyValuePair<string,ContextMethod>("Test 1", DoTest));
        methods.Add(new KeyValuePair<string,ContextMethod>("Test 2", DoTest));
        methods.Add(new KeyValuePair<string,ContextMethod>("Test 3", DoTest));
	}

    // Show menu
    void OnGUI()
    {
        if (showMenu)
        {
            Vector2 guiPos = cam.WorldToScreenPoint(this.transform.position);
            GUI.BeginGroup(new Rect(guiPos.x, guiPos.y, 100, 150));
            // Build list 
            string list = "";
            int i = 1;
            foreach (KeyValuePair<string, ContextMethod> m in methods)
            {
                list += i + ". " + m.Key + "\n";
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

    public void DoMethod(Actor a, int method)
    {
        methods[method].Value(a);
    }

    public void DoTest(Actor a)
    {
        Debug.Log(a.name + " called test in " + name);
    }
}
