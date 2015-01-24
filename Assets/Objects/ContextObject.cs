using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * Context object
 * Can be hovered over to show GUI menu and perform actions
 */

public class ContextObject : MonoBehaviour
{
    public delegate void ContextMethod(Actor a, int idx);

    public Camera                   cam;
    public Player                   player;
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
        player.selected = this;
    }

    private void OnMouseExit()
    {
        showMenu = false;
        if (curr == this)
            curr = null;
        if(player.selected == this) player.selected = null;
    }

    public void DoMethod(Actor a, int method)
    {
        if (method <= methods.Count) methods[method - 1].Value(a, method - 1);
    }

    public void DoTest(Actor a, int idx)
    {
        Debug.Log(a.name + " called #" + idx + " in " + name);
    }
}
