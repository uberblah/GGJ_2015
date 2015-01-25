using UnityEngine;
using System.Collections;

public class Hausenship : MonoBehaviour
{
    public Camera cam;
    public AudioClip crash;
    public Vector3 camOffset = new Vector3(0.0f, 0.0f, -5.0f);
    public float climb = 10.0f;
    public float decay = -0.1f;
    public float floor = -10.0f;
    public float ceil = 10.0f;
    public string crashScene = "test";
    bool gui = false;
    public float cutDelay = 2.0f;
    float cut = 0.0f;

    void Start()
    {
        audio.Play();
    }

    void Update()
    {
        if (gui && Time.time - cut >= cutDelay) Application.LoadLevel(crashScene);
        //should tell camera to follow
        cam.transform.position = new Vector3(transform.position.x, 0.0f, 0.0f) + camOffset;
        //should respond to user input by climbing
        if (Input.anyKeyDown)
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, climb, 0.0f);
        //should cut to crash scene if it falls too low
        if (transform.position.y < floor && !gui) Cut();
        climb -= decay * Time.deltaTime;
    }

    void OnCollisionEnter(Collision c)
    {
        if(!gui) Cut();
    }

    void OnGUI()
    {
        if(gui) GUI.Box(new Rect(10, 10, 600, 50), "CRASH!!!\n(crash landing in progress)");
    }

    void Cut()
    {
        gui = true;
        cut = Time.time;
        audio.PlayOneShot(crash);
    }
}
