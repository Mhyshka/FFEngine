using UnityEngine;
using System.Collections;

internal enum EInputKey
{
	None,
	A,
	B,
	X,
	Y
}

//Custom Inspector
public class ClickEventButton : MonoBehaviour
{
	#region Inspector Properties
	[HideInInspector]
	public string eventKey = "";
	[HideInInspector]
	public EEventType eventType = EEventType.Next;
	#endregion

	protected virtual void Awake()
	{
		if(eventType != EEventType.Custom)
			eventKey = eventType.ToString();
	}

	protected virtual void OnClick()
	{
		FFEngine.Events.FireEvent (eventKey);
	}
}
