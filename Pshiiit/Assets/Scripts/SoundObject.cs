using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour {




	// Use this for initialization
	void Start () 
	{

		AudioClip clip = GetComponent<AudioSource> ().clip;

		Destroy (gameObject, clip.length + 1f);
	}
}
