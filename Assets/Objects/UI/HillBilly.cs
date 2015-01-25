using UnityEngine;
using System.Collections;

public class HillBilly : MonoBehaviour {
    public string introScene = "Scenes/intro.unity";

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Application.LoadLevel(introScene);
    }
}
