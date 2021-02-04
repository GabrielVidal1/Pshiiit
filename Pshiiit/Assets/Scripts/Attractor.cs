using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	public float attractionSpeed;

	public float attractionRadius;

	Vector3 idealPos;

	Rigidbody playerRb;

	Player player;

	void Start()
	{
		idealPos = transform.position;
		idealPos.y = GameManager.gm.levelManager.player.transform.position.y;
		player = GameManager.gm.levelManager.player;
	}


	void Update () 
	{
		if (!Input.GetKey (KeyCode.Space)) {
			if (!player.dead) {
				Debug.Log ("ATTRACTION");

				float distance = Vector3.Distance (player.transform.position, idealPos);

				if (distance < attractionRadius) {

					if (playerRb == null)
						playerRb = player.GetComponent<Rigidbody> ();

					Vector3 force = (idealPos - player.transform.position).normalized * attractionSpeed * distance / attractionRadius;

					playerRb.velocity = force;


				}
			}
		}
		
	}
}
