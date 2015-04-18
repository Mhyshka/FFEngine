using UnityEngine;

public class BitMaskEnumAttribute : PropertyAttribute
{
	public System.Type propType;
	public BitMaskEnumAttribute(System.Type aType)
	{
		propType = aType;
	}
}