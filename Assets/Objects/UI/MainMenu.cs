using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string mainLevel = "test";
    public Image logo;
    public Text text;
    public Text conclusion;
    public string[] strings;
    public int[] durations;

    int stage; //0 to #strings - 1 are the strings, #strings is logo
    bool fadeout = false;
    float timer = 0.0f;

    void Start()
    {
        fadeout = false;
        stage = 0;
        text.text = strings[stage];
        timer = Time.time + durations[stage];
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }

    void Update()
    {
        if (timer < Time.time)
        {
            if (fadeout)
            {
                timer = Time.time + durations[stage];
                fadeout = false;
                if (stage == strings.Length - 1)
                {
                    conclusion.text = strings[stage];
                    conclusion.color = new Color(conclusion.color.r, conclusion.color.g, conclusion.color.b, 1.0f);
                    logo.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else
                {
                    text.text = strings[stage];
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
                }
            }
            else
            {
                stage++;
                if (stage == strings.Length)
                {
                    conclusion.text = "LOADING...";
                    Application.LoadLevel(mainLevel);
                }
                else
                {
                    timer = Time.time + 1.0f;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
                }
                fadeout = true;
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        Application.LoadLevel(mainLevel);
    }
}
