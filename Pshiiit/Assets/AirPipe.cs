using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPipe : MonoBehaviour {

	public float airFlux;


	AudioSource source;



	void Start()
	{

		source = GetComponent<AudioSource> ();
	}


	void OnTriggerStay( Collider col )
	{
		if (col.tag == "Player") {



			Player player = col.GetComponent<Player> ();
			if (player.air < player.totalAir )
				player.air += Time.deltaTime * airFlux;
			player.air = Mathf.Min (player.air, player.totalAir);

		}
	}

	void OnTriggerEnter( Collider col )
	{
		if (col.tag == "Player") {

			source.Play ();
		}
	}
}
