using UnityEditor;
using UnityEngine;

public class GUIDToAssetPath : EditorWindow
{
	string guid = "";
	string path = "";
	[MenuItem("ACT/GUIDToAssetPath")]
	static void CreateWindow()
	{
		GUIDToAssetPath window = (GUIDToAssetPath)EditorWindow.GetWindowWithRect(typeof(GUIDToAssetPath), new Rect(0, 0, 800, 150));
	}

	void OnGUI()
	{
		GUILayout.Label("Enter guid");
		guid = GUILayout.TextField(guid);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Get Asset Path", GUILayout.Width(120)))
			path = GetAssetPath(guid);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Abort", GUILayout.Width(120)))
			Close();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Label(path);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Find Missing Prefab", GUILayout.Width(120)))
			FindMissingPrefab();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void FindMissingPrefab()
	{
		string[] guids = AssetDatabase.FindAssets("t:Prefab");

		foreach (var guid in guids)
		{
			var path = AssetDatabase.GUIDToAssetPath(guid);
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

			if (go.name.Contains("Missing Prefab"))
			{
				Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
			}

			foreach (Transform t in go.transform)
			{
				if (t.name.Contains("Missing Prefab"))
				{
					Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
				}
			}
		}
	}

	static string GetAssetPath(string guid)
	{
		string p = AssetDatabase.GUIDToAssetPath(guid);
		Debug.Log(p);
		if (p.Length == 0) p = "not found";
		return p;
	}
}