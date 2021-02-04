using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public float repulsionPower;
	public bool detectable;

	public bool isMoveable;

	Rigidbody rb;

	void Start()
	{
		if (isMoveable)
			rb = GetComponent<Rigidbody> ();
	}

	public void Addforce(Vector3 force)
	{
		force.y = 0;
		rb.AddForce (force);
	}

}
