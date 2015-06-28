using UnityEngine;
using System.Collections;

public class PlayerColorFeedbackSettings : ScriptableObject
{
	[System.Serializable]
	public class ColorByBehaviour
	{
		public Color ennemi;
		public Color friendly;
		public Color neutral;
		public Color objects;
	}
	
	public ColorByBehaviour highlighted = new ColorByBehaviour();
	public ColorByBehaviour hovered = new ColorByBehaviour();
	public ColorByBehaviour selected = new ColorByBehaviour();
}
