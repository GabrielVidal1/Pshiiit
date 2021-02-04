#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class MenuSceneSwitcher : EditorWindow {

	List<FileInfo> sceneList;
	List<string> sceneNames;

	private List<bool> confirmation;

	Vector2 scrollVector;
	Vector2 scrollVector2;

	bool autosaveToggle;
	float autosaveDelay;

	float timer;


	[MenuItem("Tools/Scene Switcher")]
	static void Init()
	{
		MenuSceneSwitcher window = (MenuSceneSwitcher)GetWindow (typeof( MenuSceneSwitcher ));
		window.Show ();
	}

	void OnDestroy()
	{

	}

	void SceneSwitcherBlock()
	{
		if (sceneList == null) {
			Actualize();
		}
		EditorGUILayout.BeginHorizontal ();

		GUIStyle centeredStyle = GUI.skin.GetStyle ("label");
		centeredStyle.alignment = TextAnchor.UpperCenter;
		centeredStyle.fontSize = 12;

		EditorGUILayout.LabelField ("Chose a scene to open", centeredStyle);

		if (GUILayout.Button ("Actualize", GUILayout.Width(80f))) {
			Actualize ();
		}


		EditorGUILayout.EndHorizontal ();



		for(int i = 0 ; i < sceneList.Count ; i ++) {

			FileInfo fi = sceneList[i];

			EditorGUILayout.BeginHorizontal ();

			string sName = sceneNames [i];
			if (GUILayout.Button (sName, GUILayout.MaxHeight (30))) {
				EditorSceneManager.SaveOpenScenes ();
				EditorSceneManager.OpenScene (fi.FullName);
				EndConfirmation ();
			}
			if (!confirmation[i]) {
				if (GUILayout.Button ("del", GUILayout.Width (50f), GUILayout.MaxHeight(30))) {
					EndConfirmation ();
					confirmation[i] = true;
				}
			} else {
				if (GUILayout.Button ("SURE?", GUILayout.Width (50f), GUILayout.MaxHeight(30))) {
					File.Delete (fi.FullName);
					Actualize ();
				}
			}
			EditorGUILayout.EndHorizontal ();
		}


		EditorGUILayout.Separator ();


		if (GUILayout.Button ("New Scene",GUILayout.Height(30))) {
			CreateNewScene ();
		}

	}


	void AutosaveBlock()
	{

		if (GUILayout.Button ("Save Scene", GUILayout.MaxHeight( 20 )))
			EditorSceneManager.SaveOpenScenes ();



		autosaveToggle = EditorGUILayout.BeginToggleGroup ("Autosave", autosaveToggle);

		EditorGUILayout.LabelField ("Autosave Frequency : "+((int)(1f/(autosaveDelay/3600f))).ToString() + " save every hour");

		autosaveDelay = EditorGUILayout.Slider (autosaveDelay, 30f, 60 * 30);

		float nSave = (float)EditorApplication.timeSinceStartup - timer;

		EditorGUILayout.LabelField ("Next save in " + (int)(10f * nSave) / 10f + " seconds");
			/*
		if (autosaveDelay + timer < (float)EditorApplication.timeSinceStartup) {

			timer = (float)EditorApplication.timeSinceStartup;

			EditorSceneManager.SaveOpenScenes ();
		}
*/


		EditorGUILayout.EndToggleGroup ();

	}

	void OnGUI()
	{
		scrollVector = GUILayout.BeginScrollView (scrollVector, false, false, GUILayout.MaxHeight( Screen.height - 200));

		SceneSwitcherBlock ();

		GUILayout.EndScrollView ();

		scrollVector2 = GUILayout.BeginScrollView (scrollVector, false, false, GUILayout.MaxHeight( 200 ));

		AutosaveBlock ();

		GUILayout.EndScrollView ();


	}

	void EndConfirmation()
	{
		for (int i = 0; i < confirmation.Count; i++)
			confirmation [i] = false;
	}

	void CreateNewScene()
	{
		EndConfirmation ();
		Scene nScene = EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
		EditorSceneManager.SaveScene (nScene);
		EditorSceneManager.sceneSaved += Actualize;


	}

	public void Actualize(Scene scene) {
		Actualize ();
	}

	void Actualize()
	{
		sceneList = new List<FileInfo> ();
		sceneNames = new List<string> ();
		confirmation = new List<bool> ();

		DirectoryInfo info = new DirectoryInfo (@"D:\UNITY_DATA\Unity Projects\SKYCRAFT\Assets\Scenes");
		FileInfo[] fileInfo = info.GetFiles ();

		for (int i = 0; i < fileInfo.Length; i++) {
			if (!fileInfo [i].Name.Contains ("meta")) {
				sceneList.Add (fileInfo [i]);

				sceneNames.Add( fileInfo[i].Name.Substring (0, fileInfo[i].Name.Length - 6));

				confirmation.Add (false);
			}
		}
	}

}




#endif