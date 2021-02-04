using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheckPoint : MonoBehaviour {

	public bool isTheEndOfTheLevel;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {
			GameManager.gm.levelManager.actualRespawnPoint = transform.position;
			if (isTheEndOfTheLevel) {
				if (!col.GetComponent<Player> ().dead) {
					GameManager.gm.levelManager.FinishLevel ();
				}
			}
		}
	}



}
