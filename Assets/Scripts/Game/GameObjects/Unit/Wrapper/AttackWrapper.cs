using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EAttackStrikeType
{
	Normal,
	Penetration,
	Crititcal
}

public class AttackWrapper
{

	#region Inspector Properties
	#endregion

	#region Properties
	internal string name = "";
	internal Vector3 targetPosition = Vector3.zero;
	internal Unit source = null; 
	internal EAttackStrikeType strikeType;
	internal List<DamageWrapper> damages = null;
	#endregion

	#region Methods
	#endregion
}