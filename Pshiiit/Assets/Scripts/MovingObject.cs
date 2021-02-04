using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public Transform[] points;

	public Transform obj;

	public float speed;

	public float waitingTimeAtPoint;

	int currentPoint;

	int goToPoint;

	float t = 0;

	// Use this for initialization
	void Start () {
		if (points.Length < 2)
			throw new UnityException ("Il faut au moins 2 points !!!");


		currentPoint = 0;

		goToPoint = 1;

		StartCoroutine (Go ());

	}

	IEnumerator Go()
	{
		while (true) {

			Vector3 fromPoint = points [currentPoint].position;
			
			Vector3 goPoint = points [goToPoint].position;
			
			float time = Vector3.Distance (fromPoint, goPoint) / speed;

			t = 0f;

			while (t < 1f) {

				obj.transform.position = (1 - t) * fromPoint + t * goPoint;
				
				t += 0.01f;

				yield return new WaitForSeconds (0.01f * time);
			}

			currentPoint = goToPoint;

			goToPoint++;

			if (goToPoint > points.Length - 1)
				goToPoint = 0;


			yield return new WaitForSeconds (waitingTimeAtPoint);


		}
	}


}
