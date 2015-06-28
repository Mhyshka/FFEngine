using UnityEngine;
using System.Collections;

public enum EOwnerStatus
{
	Ennemi,
	Neutral,
	Friendly,
	Player
}

public class Owner
{
	#region Properties
	internal EOwnerStatus status = EOwnerStatus.Neutral;
	
	protected string _name = "Owner";
	internal string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
	#endregion
}
