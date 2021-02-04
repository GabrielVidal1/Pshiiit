using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCactus : MonoBehaviour {

	public Transform foot;

	public GameObject floatingPot;

	public Transform head1;
	public Transform head2;
	public Transform head3;

	Obstacle obstacle;

	public Vector3 initialPos;

	void Start () 
	{
		initialPos = transform.position;

		obstacle = GetComponent<Obstacle> ();
		if (obstacle.isMoveable) {
			floatingPot.SetActive (true);
			foot.gameObject.SetActive (false);

			GameManager.gm.levelManager.movingCactus.Add (this);


		}
		ProceduralMe ();
		
	}

	void ProceduralMe()
	{
		foot.localRotation = RandomRot (-15f, 15f);

		if (Random.value < 0.5f) {

			head1.gameObject.SetActive (true);

			head1.rotation = RandomRot (-10f, 10f);

			if (Random.value < 0.5f) {

				head2.gameObject.SetActive (true);

				head2.rotation = RandomRot (-10f, 10f);

				if (Random.value < 0.5f) {

					head3.gameObject.SetActive (true);

					head3.rotation = RandomRot (-10f, 10f);


				}
			}
		}
	}


	Quaternion RandomRot( float min, float max )
	{
		return Quaternion.Euler (new Vector3 (Random.Range (min, max), Random.Range (min, max), Random.Range (min, max)));
	}
}
