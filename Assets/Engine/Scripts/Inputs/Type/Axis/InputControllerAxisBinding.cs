using UnityEngine;
using System.Collections;

namespace FF.Input
{
	[System.Serializable]
	internal class InputControllerAxisBinding
	{
	#region Properties
		[Range(1,11)]
		public int controllerIndex;
		
		[Range(1,20)]
		public int axisIndex;
	#endregion
	
	#region Methods
		internal InputControllerAxisBinding(int a_ctrlIndex, int a_axisIndex, bool isInverted = false)
		{
			controllerIndex = a_ctrlIndex;
			axisIndex = a_axisIndex;
		}
		
		internal string Name
		{
			get
			{
				return "Joystick" + controllerIndex.ToString() + "Axis" + axisIndex.ToString();
			}
		}
		
		internal float Value
		{
			get
			{
				return UnityEngine.Input.GetAxis(Name);
			}
		}
	#endregion
	
		internal static InputControllerAxisBinding Current
		{
			get
			{
				InputControllerAxisBinding binding = null;
				for(int i = 1 ; i < 2 ; i++)
				{
					for(int j = 1 ; j < 9 ; j++)
					{
						if(Mathf.Abs(UnityEngine.Input.GetAxisRaw("Joystick" + i + "Axis" + j)) > 0.3f)
						{
							binding = new InputControllerAxisBinding(i,j);
							return binding;
						}
					}
				}
				
				return binding;
			}
		}
	}
}
