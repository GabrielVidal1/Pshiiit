using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public Text levelName;

	public int level;

	public GameObject lockIcon;

	Button button;


	void Start()
	{
		button = GetComponent<Button> ();

		int maxLevel = GameManager.gm.maxLevel;
		if (level <= maxLevel) {
			lockIcon.SetActive (false);
			levelName.gameObject.SetActive (true);
			button.enabled = true;
		} else {
			button.enabled = false;
			lockIcon.SetActive (true);
			levelName.gameObject.SetActive (false);
		}

		SetLevelToLoad ();
	}

	void SetLevelToLoad()
	{
		levelName.text = "Level " + level.ToString ();
		level = level;

		button.onClick.RemoveAllListeners ();
		button.onClick.AddListener (Level);
	}

	void Level()
	{
		GameManager.gm.LoadLevel (level);
	}

}
