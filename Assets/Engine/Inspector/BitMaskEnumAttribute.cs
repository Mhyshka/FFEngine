using UnityEngine;

namespace FFEngine
{
	public class BitMaskEnumAttribute : PropertyAttribute
	{
		public System.Type propType;
		public BitMaskEnumAttribute(System.Type aType)
		{
			propType = aType;
		}
	}
}