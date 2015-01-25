using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string mainLevel = "test";

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Application.LoadLevel(mainLevel);
    }
}
