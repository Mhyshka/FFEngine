using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class EffectConf
{
	internal abstract Effect Compute(Unit a_source);
}
