using UnityEngine;
using System.Collections;

public class IntRange : IntValue
{
	public IntValue min = null;
	public IntValue max = null;
	
	
	internal override int Value
	{
		get
		{
			return Random.Range(min.Value,
								max.Value + 1);
		}
	}
}
