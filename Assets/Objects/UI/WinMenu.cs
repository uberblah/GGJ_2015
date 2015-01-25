using UnityEngine;
using System.Collections;

public class WinMenu : MonoBehaviour
{
    public string initLevel = "hillbilly";

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Application.LoadLevel(initLevel);
    }
}
