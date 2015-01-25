using UnityEngine;
using System.Collections;

public class HillBilly : MonoBehaviour {

    public AudioSource menuMusic;
    public string introScene = "intro";

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Application.LoadLevel(introScene);
    }
}
