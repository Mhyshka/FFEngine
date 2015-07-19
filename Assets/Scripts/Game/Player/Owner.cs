using UnityEngine;
using System.Collections;
using FullInspector;

public enum EOwnerStatus
{
	Ennemi,
	Neutral,
	Friendly,
	Player
}

public class Owner : FullInspector.BaseBehavior
{
	#region Properties
	public EOwnerStatus status = EOwnerStatus.Neutral;
	
	public string Name
	{
		get
		{
			return gameObject.name;
		}
		set
		{
			gameObject.name = value;
		}
	}
	#endregion
	
	protected virtual void Awake()
	{
	
	}
}
