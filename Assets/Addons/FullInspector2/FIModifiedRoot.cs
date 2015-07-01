using UnityEngine;
using System.Collections;
using FullInspector;

public class UpdateFullInspectorRootDirectory : fiSettingsProcessor
{
	public void Process() 
	{
		fiSettings.RootDirectory = "Assets/Addons/FullInspector2/";
	}
}