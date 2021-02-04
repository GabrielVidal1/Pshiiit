using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour {

	[HideInInspector]
	public float windForce;


	void OnTriggerStay(Collider col)
	{
		if (col.tag == "Player") {
			col.GetComponent<Rigidbody> ().AddForce (transform.forward * windForce);
		}
	}

}
