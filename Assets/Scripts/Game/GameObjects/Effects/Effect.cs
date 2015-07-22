using UnityEngine;
using System.Collections;

public abstract class Effect
{
	internal Unit source;
	
	internal abstract void Apply(Unit a_target);
}
