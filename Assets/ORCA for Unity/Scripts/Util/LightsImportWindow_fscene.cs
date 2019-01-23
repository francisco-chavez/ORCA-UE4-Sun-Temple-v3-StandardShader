
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;


public class LightsImportWindow_fscene 
	: EditorWindow
{

	private static string _filePath = string.Empty;


	[MenuItem("Window/ORCA/Import Lights")]
	static void Init()
	{
		var window = EditorWindow.GetWindow<LightsImportWindow_fscene>();
		window.Show();
	}

	private void OnEnable()
	{
		if (string.IsNullOrEmpty(_filePath))
			_filePath = Application.dataPath;
	}

	private void OnGUI()
	{
		GUILayout.Label("File Select:", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fscene path:", GUILayout.ExpandWidth(false));
		EditorGUILayout.TextField(_filePath, GUILayout.ExpandWidth(true));
		GUILayout.EndHorizontal();

		bool openFileBrowswer = GUILayout.Button("Browse...", GUILayout.ExpandWidth(false));

		if (openFileBrowswer)
		{
			var directory = Application.dataPath;

			if (Directory.Exists(_filePath))
			{
				directory = _filePath;
			}
			else if (File.Exists(_filePath))
			{
				var fileName = Path.GetFileName(_filePath);
				directory = _filePath.Substring(0, _filePath.Length - fileName.Length);
			}

			var pathT = EditorUtility.OpenFilePanel("Select fscene", directory, "fscene");

			_filePath = string.IsNullOrEmpty(pathT)
					  ? _filePath
					  : pathT;
		}

		// Do some basic checking to make sure there is an fscene before giving the User 
		// the option of importing from it.
		if (!File.Exists(_filePath))
			return;
		if (!_filePath.EndsWith(".fscene", System.StringComparison.OrdinalIgnoreCase))
			return;

		GUILayout.Space(10);
		bool importLights = GUILayout.Button("Import Lights");
	
		if (!importLights)
			return;
	}
}
