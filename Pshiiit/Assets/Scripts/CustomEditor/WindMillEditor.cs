#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor( typeof( WindMill ) ) ]
public class WindMillEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		WindMill myObject = (WindMill)target;

		if (GUILayout.Button ("Preview")) {

			myObject.UpdateZone ();
		}


	}
}
#endif