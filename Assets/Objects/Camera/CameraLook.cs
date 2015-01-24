using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour
{
    public GameObject primary = null;
    public Camera viewer = null;

    public float tension = 3.0f;
    public float limit = 1000.0f;
    public float lookWeight = 0.25f;

	// Use this for initialization
	void Start ()
    {
        if (primary == null) Debug.Log(name + " has no assigned primary target!");
        if (viewer == null)
        {
            viewer = GetComponent<Camera>();
            if (viewer == null) Debug.Log(name + " couldn't find its camera!");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 us = transform.position;
        Vector3 them = primary.transform.position;
        Vector3 mean = them +
            ((viewer.ScreenToWorldPoint(Input.mousePosition) - them) * lookWeight);
        Vector3 diff = mean - us;
        diff.z = 0.0f;
        Vector3 newpos = us + (diff * tension * Time.deltaTime);
        newpos.z = us.z;
        transform.position = newpos;
	}
}
