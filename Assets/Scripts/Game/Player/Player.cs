using UnityEngine;
using System.Collections;

public class Player : Owner
{
	#region Inspector Properties
	public Color color = Color.blue;
	public Unit hero = null;
	#endregion
	
	#region Properties
	public PlayerSelectionScript selection;
	public PlayerOrderScript order;
	#endregion
	
	#region Init
	protected override void Awake ()
	{
		base.Awake ();
		FindPlayerScripts();
	}
	
	internal void FindPlayerScripts()
	{
		/*foreach(APlayerScript each in GetComponents<APlayerScript>())
		{
			ParsePlayerCallbacks(each);
			each.Init(this);
		}*/
		
		foreach(APlayerScript each in GetComponentsInChildren<APlayerScript>())
		{
			ParsePlayerCallbacks(each);
			each.Init(this);
		}
	}
	
	internal void ParsePlayerCallbacks(APlayerScript a_script)
	{
	}
	#endregion
}
