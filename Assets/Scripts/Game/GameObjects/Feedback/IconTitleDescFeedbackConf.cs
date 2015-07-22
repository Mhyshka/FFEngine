using UnityEngine;
using System.Collections;

[System.Serializable]
public class IconTitleDescFeedbackConf
{
	/// <summary>
	/// The atlas in which to look for the sprite name. Use to display effect icon. If none, won't be displayed.
	/// </summary>
	public UIAtlas atlas = null;
	
	/// <summary>
	/// The name of the sprite in the atlas.
	/// </summary>
	public string spriteName = "none";
	
	/// <summary>
	/// The localization key used for this effect title.
	/// </summary>
	public string nameLocKey = "title";
	
	/// <summary>
	/// The localization key used for this effect description.
	/// </summary>
	public string descriptionLocKey = "description";
}
