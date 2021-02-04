using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject levelSelectionPanel;
	public GameObject optionsMenu;

	public GameObject levelParent;
	public GameObject levelButtonPrefab;

	public Slider volumeSlider;

	AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource> ();
		CreateLevelArray ();
	}

	public void ButtonSound()
	{
		source.Play ();
	}

	public void LoadLevel( int index )
	{
		GameManager.gm.LoadLevel (index);
	}


	public void MainMenuToggle()
	{
		levelSelectionPanel.SetActive (false);
		optionsMenu.SetActive (false);

		mainMenu.SetActive (true);
	}

	void CreateLevelArray()
	{
		int nbLevel = SceneManager.sceneCountInBuildSettings - 1;

		for( int i = 0 ; i < nbLevel ; i ++ )
		{
			GameObject levelButton = (GameObject) Instantiate( levelButtonPrefab, levelParent.transform );

			LevelButton lb = levelButton.GetComponent<LevelButton> ();

			lb.level = i + 1;

		}

		//throw new UnityException ("à finir !!!");
	}


	public void OptionsMenu()
	{
		mainMenu.SetActive (false);

		optionsMenu.SetActive (true);
	}


	public void LevelSelectionPanel()
	{
		mainMenu.SetActive (false);

		levelSelectionPanel.SetActive (true);

	}

	public void UpdateMasterVolume()
	{

		AudioListener.volume = volumeSlider.value;


	}

	public void ResetProgression()
	{
		GameManager.gm.FirstBoot ();	
		GameManager.gm.Save ();
		MainMenuToggle ();

	}

	public void QuitButton()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit ();
		#endif
	}
}
