using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public Vector3 actualRespawnPoint;

	public OnPlayCanvas canvas;

	public Player player;

	public List<GameObject> collectibles;

	public List<SpikeCactus> movingCactus;

	public int level;

	void Awake()
	{
		collectibles = new List<GameObject> ();
		movingCactus = new List<SpikeCactus> ();

	}

	void Start()
	{


		actualRespawnPoint = player.transform.position;

		string sceneName = SceneManager.GetActiveScene ().name;
		level = int.Parse( sceneName.Substring (6, sceneName.Length - 6) );

		GameObject[] t = GameObject.FindGameObjectsWithTag ("Collectible");

		foreach (GameObject a in t)
			collectibles.Add (a);
	}

	public void ResetCollectibles()
	{
		foreach (GameObject obj in collectibles) {
			obj.SetActive (true);
		}
		foreach (SpikeCactus cactus in movingCactus) {
			cactus.transform.position = cactus.initialPos;
			cactus.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		}

	}

	public void FinishLevel()
	{

		GameManager.gm.PlaySound (Sounds.Winning, player.transform.position);

		//Display canvas

		canvas.Win ();

		Debug.Log ("END!!!");

		Time.timeScale = 0f;

		GameManager.gm.Save ();
	}

	public void LoadNextLevel()
	{
		Time.timeScale = 1f;
		GameManager.gm.LoadLevel (level + 1);
	}

}
