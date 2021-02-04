using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public float airBonus;

	public bool power;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {

			if (airBonus > 0) {
				col.GetComponent<Player> ().air = Mathf.Min (col.GetComponent<Player> ().air + airBonus, col.GetComponent<Player> ().totalAir);

				GameObject t = GameManager.gm.PlaySound (Sounds.BlowingBalloon, transform.position);
			}

			if (power) {
				col.GetComponent<Player> ().HasSuperPowers (true);
			}

			gameObject.SetActive (false);
		}
	}
}
