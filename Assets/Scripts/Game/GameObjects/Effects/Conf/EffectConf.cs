using UnityEngine;
using System.Collections;
using FullInspector;

[System.Serializable]
public abstract class EffectConf
{
	internal abstract Effect Compute(Unit a_source);
}
