using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMill : MonoBehaviour {

	public ParticleSystem particles;

	public BoxCollider effectZone;

	public float effectLength;

	public float windForce;

	// Use this for initialization
	public void UpdateZone () 
	{
		Vector3 pos = effectZone.transform.localPosition;
		pos.z = effectLength / 2f;
		effectZone.transform.localPosition = pos;

		Vector3 size = effectZone.size;
		size.z = effectLength;
		effectZone.size = size;

		effectZone.GetComponent<WindZone> ().windForce = windForce;

		ParticleSystem.MainModule main = particles.main;

		ParticleSystem.MinMaxCurve lt = main.startLifetime;

		lt.constant = effectLength / particles.main.startSpeed.constant;

		main.startLifetime = lt;
	}

	void Start()
	{
		UpdateZone ();
	}


}
