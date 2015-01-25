using UnityEngine;
using System.Collections;

public class Hausenship : MonoBehaviour
{
    public Camera cam;
    public Vector3 camOffset = new Vector3(0.0f, 0.0f, -5.0f);
    public float climb = 1.0f;
    public float floor = -10.0f;
    public string crashScene = "crash";

    void Update()
    {
        //should tell camera to follow
        cam.transform.position = transform.position + camOffset;
        //should respond to user input by climbing
        if (Input.anyKeyDown) rigidbody.velocity = new Vector3(rigidbody.velocity.x, climb, 0.0f);
        //should point in its direction of travel
        
        //should cut to crash scene if it falls too low
        if (transform.position.y < floor) Application.LoadLevel(crashScene);
        //should cut to crash scene if it hits anything

    }
}
