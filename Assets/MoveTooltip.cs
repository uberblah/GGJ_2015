using UnityEngine;
using System.Collections;

public class MoveTooltip : MonoBehaviour {

    public float width = 300;
    public float height = 20;
    public string message = "WASD to move, Mouse to fire";

    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height), message);
    }
}
