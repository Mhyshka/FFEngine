using UnityEngine;
using System.Collections;

public class UnitTimeHandler : AUnitComponent
{
	protected float _effectTimScale = 1f;
	internal float EffectTimeScale
	{
		get
		{
			return _effectTimScale;
		}
	}
}
