using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Linq;

public enum EIntValueTest
{
	None,
	Hardcode,
	Arithmetic
}

//[CustomPropertyDrawer(typeof(IntValue))]
public class IntValueDrawer : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property);
		//return base.GetPropertyHeight (property, label) + 15f;
	}
	
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		base.OnGUI(position, property, label);
		/*EditorGUI.PrefixLabel(position, new GUIContent(property.displayName + " : " + property.propertyType.ToString()));

		EditorGUI.BeginProperty(position, label, property);
		
		EditorGUI.PropertyField(position,
		                        property,
		                        true);
		                       
		System.Type[] types = System.Reflection.Assembly.GetAssembly(fieldInfo.FieldType).GetTypes();
		System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(fieldInfo.FieldType) select type).ToArray();
		string[] names = new string[possible.Length];
		
		for(int i = 0 ; i < possible.Length ; i++)
		{
			names[i] = possible[i].ToString();
		}
		//EditorGUI.Popup(position,0,names);

		EditorGUI.EndProperty();*/
	}

}

/*
[CustomEditor(typeof(MonoBehaviour), true)]
public class IntValueEditor : Editor
{
	public EIntValueTest type = EIntValueTest.None;
	
	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		Debug.Log("test");
		
		EditorGUILayout.LabelField(target.ToString());
		
		if(GUILayout.Button("Next"))
		{
			if(type == EIntValueTest.None)
				type = EIntValueTest.Hardcode;
			else
				type = EIntValueTest.None;
		}
		
		switch(type)
		{
			case EIntValueTest.None : 
			break;
			
			case EIntValueTest.Hardcode :
			break;
		}
		//foreach(SerializeField field in script.)
	}
}*/
