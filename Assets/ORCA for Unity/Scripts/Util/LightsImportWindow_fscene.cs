
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


public class LightsImportWindow_fscene 
	: EditorWindow
{
	[MenuItem("Window/ORCA/Import Lights")]
	static void Init()
	{
		var window = EditorWindow.GetWindow<LightsImportWindow_fscene>();
		window.Show();
	}
}
