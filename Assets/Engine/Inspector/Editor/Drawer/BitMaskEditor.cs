using UnityEngine;
using UnityEditor;
using System.Collections;
using FF;

public static class EditorExtension
{
	public static int DrawBitMaskField (Rect aPosition, int aMask, System.Type aType, GUIContent aLabel)
	{
		var itemNames = System.Enum.GetNames(aType);
		var itemValues = System.Enum.GetValues(aType) as int[];
		
		int val = aMask;
		int maskVal = 0;
		for(int i = 0; i < itemValues.Length; i++)
		{
			if (itemValues[i] != 0)
			{
				if ((val & itemValues[i]) == itemValues[i])
					maskVal |= 1 << i;
			}
			else if (val == 0)
				maskVal |= 1 << i;
		}
		int newMaskVal = EditorGUI.MaskField(aPosition, aLabel, maskVal, itemNames);
		int changes = maskVal ^ newMaskVal;
		for(int i = 0; i < itemValues.Length; i++)
		{
			if ((changes & (1 << i)) != 0) // has this list item changed?
			{
				if ((newMaskVal & (1 << i)) != 0) // has it been set?
				{
					if (itemValues[i] == 0) // special case: if "0" is set, just set the val to 0
					{
						val = 0;
						break;
					}
					else
						val |= itemValues[i];
				}
				else // it has been reset
				{
					val &= ~itemValues[i];
				}
			}
		}
		return val;
	}
	
	public static int DrawBitMaskField (Rect aPosition, int aMask, string[] a_scenes, GUIContent aLabel)
	{
		int[] values = new int[a_scenes.Length];
		for(int i = 0; i < a_scenes.Length; i++)
		{
			values[i] = 1 << i;
		}
		
		int val = aMask;
		int maskVal = 0;
		for(int i = 0; i < a_scenes.Length; i++)
		{
			if (values[i] != 0)
			{
				if ((val & values[i]) == values[i])
					maskVal |= 1 << i;
			}
			else if (val == 0)
				maskVal |= 1 << i;
		}
		int newMaskVal = EditorGUI.MaskField(aPosition, aLabel, maskVal, a_scenes);
		int changes = maskVal ^ newMaskVal;
		for(int i = 0; i < values.Length; i++)
		{
			if ((changes & (1 << i)) != 0) // has this list item changed?
			{
				if ((newMaskVal & (1 << i)) != 0) // has it been set?
				{
					if (values[i] == 0) // special case: if "0" is set, just set the val to 0
					{
						val = 0;
						break;
					}
					else
						val |= values[i];
				}
				else // it has been reset
				{
					val &= ~values[i];
				}
			}
		}
		
		return val;
	}
}

[CustomPropertyDrawer(typeof(BitMaskEnumAttribute))]
public class EnumBitMaskEnumPropertyDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label)
	{
		var typeAttr = attribute as BitMaskEnumAttribute;
		// Add the actual int value behind the field name
		label.text = label.text + "("+prop.intValue+")";
		prop.intValue = EditorExtension.DrawBitMaskField(position, prop.intValue, typeAttr.propType, label);
	}
}

[CustomPropertyDrawer(typeof(BitMaskUIScenesAttribute))]
public class EnumBitMaskScenesPropertyDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label)
	{
		BitMaskUIScenesAttribute typeAttr = attribute as BitMaskUIScenesAttribute;
		// Add the actual int value behind the field name
		label.text = label.text + "("+prop.intValue+")";
		prop.intValue = EditorExtension.DrawBitMaskField(position, prop.intValue, typeAttr.scenes, label);
	}
}