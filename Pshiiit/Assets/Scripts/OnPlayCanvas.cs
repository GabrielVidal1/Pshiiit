using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnPlayCanvas : MonoBehaviour {

	public GameObject pauseMenu;
	public GameObject deathCanvas;
	public GameObject winCanvas;
	public GameObject endCanvas;

	public Text respawnTimer;
	public Image respawnPieChart;

	public bool toggle = true;
	bool win = false;

	bool end = false;

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			TogglePause (toggle);
		}

		if (win) {
			
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (end) 
					MainMenu ();
				else
					GameManager.gm.levelManager.LoadNextLevel ();
			}
		}
	}

	public void TogglePause( bool toggle )
	{
		if (toggle) {
			Time.timeScale = 0f;
			pauseMenu.SetActive (toggle);
			toggle = false;
		} else {
			Time.timeScale = 1f;
			pauseMenu.SetActive (toggle);
			toggle = true;
		}
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene ("MainMenu");
	}

	public void ToggleDeathCanvas (float respawnTime)
	{
		deathCanvas.SetActive (true);
		StartCoroutine (TimerRespawn (respawnTime));
	}


	IEnumerator TimerRespawn(float respawnTime)
	{
		Debug.Log (respawnTime);

		for (float i = respawnTime; i > 0; i -= 0.01f) {
			respawnTimer.text = ((int)(i*10f)/10f).ToString ();
			respawnPieChart.fillAmount = i / respawnTime;
			yield return new WaitForSeconds (0.01f);
		}
		deathCanvas.SetActive (false);
		GameManager.gm.levelManager.player.Respawn ();
	}

	public void Win()
	{
		deathCanvas.SetActive (false);
		win = true;

		Debug.Log( SceneManager.sceneCountInBuildSettings - 1 +"    "+ GameManager.gm.levelManager.level );



		if (SceneManager.sceneCountInBuildSettings - 1 == GameManager.gm.levelManager.level) {
			end = true;
			endCanvas.SetActive (true);
		} else {

			winCanvas.SetActive (true);
		}



	}


	public void RestartLevel()
	{
		GameManager.gm.levelManager.player.Die ();
		TogglePause (false);
	}

}
