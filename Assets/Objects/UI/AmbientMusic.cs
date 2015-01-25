using UnityEngine;
using System.Collections;

public class AmbientMusic : MonoBehaviour
{
    public AudioClip[] musics;

	// Use this for initialization
	void Start ()
    {
	    // Choose a song to play
        AudioSource aSrc = GetComponent<AudioSource>();
        aSrc.clip = musics[(int)Random.Range(0, musics.Length - 1)];
        aSrc.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
