using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Transform pivot;
	public Transform instantPivot;

	public GameObject meshes;

	public GameObject trumpet;

	public GameObject missilePrefab;

	public float radius;
	public float speed;

	public float respawnTime;

	public float totalAir = 100f;
	public float airConsumation;
	public float air;

	public GameObject balloon;

	public ParticleSystem particle;
	public ParticleSystem explosionParticle;
	public ParticleSystem powerParticle;

	Animator animator;

	Vector3 target;
	Obstacle objectTargeted;

	Rigidbody rb;

	[HideInInspector]
	public bool dead;

	Vector3 normalSize;

	float normalColliderRadius;

	bool canUpdateSize;

	float maxConeAngle;

	Vector3 maxTrumpetSize;
	Vector3 minTrumpetSize;

	float timer;
	float maxTimer = 3f;

	bool goTimer;


	bool hasSuperPower = false;


	AudioSource audioSource;
	float minimumVolume = 0.1f;

	// Use this for initialization
	void Start () {

		audioSource = GetComponent<AudioSource> ();




		maxTrumpetSize = trumpet.transform.localScale;
		minTrumpetSize = Vector3.one * 0.1f;
		minTrumpetSize.z = 0f;

		maxConeAngle = particle.shape.angle;

		normalColliderRadius = GetComponent<SphereCollider> ().radius;

		rb = GetComponent<Rigidbody> ();
		GameManager.gm.levelManager.player = this;

		normalSize = balloon.transform.localScale;
		animator = GetComponent<Animator> ();

		
		
		StartCoroutine (ApprochPos ());
		StartCoroutine (GetToSize ());
	}



	// Update is called once per frame

	void TargetClosest()
	{
		
		Collider[] cols = Physics.OverlapSphere (transform.position, radius);
		
		if (cols.Length > 0) {
			float min = radius + 1;
			float min2 = min + 1;
			Vector3 closestObstacle = Vector3.zero;
			Vector3 closestObstacle2 = Vector3.zero;

			GameObject targetGamobject = null;

			float repPower = 0f;
			
			foreach (Collider col in cols) {
				if (col.tag == "Obstacle") {
					
					Obstacle obstacle = col.GetComponent<Obstacle> ();
					if (obstacle.detectable) {
						
						float distance = Vector3.Distance (col.transform.position, transform.position);
						
						if (distance < min) {
							targetGamobject = col.gameObject;
							min = distance;
							closestObstacle = col.transform.position;
							repPower = obstacle.repulsionPower;
						}
						if (distance < min2 && distance > min) {
							min2 = distance;
							closestObstacle2 = col.transform.position;
							
						}
					}
				}
			}
			if (targetGamobject != null)
				objectTargeted = targetGamobject.GetComponent<Obstacle>();

			/*
			float ca = 1 / min;
			float cb = 1 / min2;

			float s = ca + cb;

			target = (ca * closestObstacle + cb * closestObstacle2) / s;
			*/
			
			target = closestObstacle;
			
			target.y = pivot.position.y;


			instantPivot.LookAt (target);
			
			
		}
		
	}

	void Shoot ()
	{

		GameObject missile = (GameObject)Instantiate (missilePrefab, particle.transform.position, Quaternion.identity);

		StartCoroutine( GoToTarget( missile.transform.position, target, missile, objectTargeted.gameObject ));
		Debug.Log ("testesstets");

		HasSuperPowers (false);

	}

	IEnumerator GoToTarget(Vector3 start, Vector3 end, GameObject missile, GameObject shootedObject)
	{
		Debug.Log ("testesstets");
		Debug.Log (start + "           " + end);

		for (float i = 0; i < 1f; i+= 0.01f) {

			missile.transform.position = (1 - i) * start + i * end;


			yield return new WaitForSeconds (0.02f);
		}

		if ( !GameManager.gm.levelManager.collectibles.Contains( shootedObject ))
			GameManager.gm.levelManager.collectibles.Add (shootedObject);

		shootedObject.SetActive (false);
		missile.SetActive (false);


	}

	void Update () {
		if (!dead) {
			//////////////////////////
			TargetClosest ();
			/////////////////////////

			if (hasSuperPower) 
			{
				particle.gameObject.SetActive (false);

				if (Input.GetKeyDown(KeyCode.Space))
					Shoot ();


			} else {


				if (!Input.GetKey (KeyCode.Space))
					goTimer = false;

				if (Input.GetKey (KeyCode.Space)) {
					if (!goTimer) {
						timer = Time.time;
						goTimer = true;
					}


					particle.gameObject.SetActive (true);
					Vector3 force = -(target - transform.position).normalized;

					float multiplier = Mathf.Sqrt ((Time.time - timer) / maxTimer);

					audioSource.volume = multiplier;

					Vector3 totalForce = force * speed * Time.deltaTime * multiplier;

					rb.AddForce (totalForce);

					if (objectTargeted.isMoveable) {

						objectTargeted.Addforce (-force);

					}


					air -= airConsumation * Time.deltaTime * multiplier;

					if (air < 0) {
						Die ();
					}

					GetComponent<SphereCollider> ().radius = normalColliderRadius * air / totalAir;

					SetConeOuverture (Time.time - timer);


				} else {
					//////////////////////////////

					//////////////////////////////
					audioSource.volume = 0f;
					particle.gameObject.SetActive (false);
				}
			}
		}
	}

	IEnumerator GetToSize()
	{
		float actualT = 0;

		float coef = 0.1f;

		animator.SetBool ("Respawn", false);

		while (!dead) {

			float t = 1 - (air / totalAir);

			actualT = (1 - coef) * actualT + coef * t;

			animator.Play ("Shrink", 0, actualT);

			yield return new WaitForSeconds (0.01f);

		}
	}

	IEnumerator ApprochPos()
	{
		while (true) {

			float coef = .1f;

			pivot.rotation = Quaternion.Euler ((1 - coef) * pivot.rotation.eulerAngles + coef * instantPivot.rotation.eulerAngles);

			yield return new WaitForSeconds (0.01f);
		}
	}

	public void HasSuperPowers(bool yes)
	{
		particle.gameObject.SetActive (false);
			
		hasSuperPower = yes;
		powerParticle.gameObject.SetActive (yes);

	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log (col.collider.tag);
		if (col.collider.tag == "Obstacle") {
			Explode ();
		}
	}

	void Repulse(Vector3 from)
	{
		rb.AddForce ((transform.position - from).normalized * 40f * speed);
	}

	public void Die()
	{
		animator.SetTrigger ("Death");
		GenericDeath ();

	}


	public void Respawn()
	{
		//meshes.SetActive (true);

		animator.SetBool ("Respawn", true);
		goTimer = false;
		transform.position = GameManager.gm.levelManager.actualRespawnPoint;
		animator.speed = 0f;

		rb.velocity = Vector3.zero ;

		GameManager.gm.levelManager.ResetCollectibles ();

		dead = false;

		StartCoroutine (GetToSize ());

	}


	void SetConeOuverture( float timer )
	{
		ParticleSystem.ShapeModule ps = particle.shape;

		ps.angle = Mathf.Min( maxConeAngle * (timer + 0.1f)/ maxTimer, maxConeAngle );

		Vector3 size = (maxTrumpetSize * timer / maxTimer) + minTrumpetSize; 

		if (size.x > maxTrumpetSize.x)
			size = maxTrumpetSize;

		trumpet.transform.localScale = size;

	}

	public void Explode()
	{
		animator.SetTrigger ("Explode");

		GenericDeath ();

	}

	public void PlayExplosion()
	{
		explosionParticle.Play ();
	}

	public void StopExplosion()
	{
		explosionParticle.Stop ();
	}

	void GenericDeath()
	{

		HasSuperPowers (false);

		StopCoroutine (GetToSize ());
		
		air = totalAir;
		animator.speed = 1f;
		audioSource.volume = 0f;

		GameManager.gm.PlaySound (Sounds.Explosing, transform.position);

		GameManager.gm.levelManager.canvas.ToggleDeathCanvas (respawnTime);
		dead = true;
		rb.velocity = Vector3.zero;
	}
}
