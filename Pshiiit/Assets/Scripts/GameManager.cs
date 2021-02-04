using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

	public static GameManager gm;

	public int maxLevel;

	public GameObject BlowingBalloonSound;
	public GameObject ExplodingSound;
	public GameObject WinningSound;



	LevelManager lm;
	public LevelManager levelManager
	{
		get 
		{
			if (lm == null)
				lm = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
			return lm;
		}
	}


	public GameObject PlaySound(Sounds sound, Vector3 position)
	{
		switch (sound) {
		case Sounds.BlowingBalloon:
			return (GameObject)Instantiate (BlowingBalloonSound, position, Quaternion.identity);
		case Sounds.Explosing :
			return (GameObject)Instantiate (ExplodingSound, position, Quaternion.identity);
		case Sounds.Winning :
			return (GameObject)Instantiate (WinningSound, position, Quaternion.identity);
		default :
			return null;
		}

	}


	void Awake()
	{
		if (gm == null)
			gm = this;
		else if (gm != this)
			Destroy (this);

		gm.Load ();

	}

	void Start()
	{

	}

	public void LoadLevel( int level )
	{
		SceneManager.LoadScene ("Level " + level);
	}

	public void Save()
	{

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create( Application.persistentDataPath + "/GameData.dat");

		GameData data = Encapsulate();


		bf.Serialize( file, data );
		file.Close();
	}

	GameData Encapsulate()
	{
		GameData data = new GameData ();
		data.maxLevel = maxLevel;
		return data;
	}


	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/GameData.dat")) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/GameData.dat", FileMode.Open); 

			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			Decapsulate (data);

		} else {

			FirstBoot();

		}

		maxLevel = 100;

	}

	void Decapsulate(GameData data)
	{

		maxLevel = data.maxLevel;


	}

	public void FirstBoot()
	{

		maxLevel = 0;

	}

}

[Serializable]
class GameData
{
	public int maxLevel;


}


public enum Sounds
{
	BlowingBalloon,
	Explosing,
	Winning
}

