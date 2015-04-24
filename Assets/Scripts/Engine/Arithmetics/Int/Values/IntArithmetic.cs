using UnityEngine;
using System.Collections;

public enum EArithmeticOperation
{
	Plus,
	Minus,
	Mult,
	Div,
	Mod,
	Max,
	Min,
	Dist
}

[System.Serializable]
public class IntArithmetic : IntValue
{

	#region Inspector Properties

	public IntValue leftVal = new IntHardcode();
	public EArithmeticOperation operation = EArithmeticOperation.Plus;
	public IntValue rightVal = null;
	#endregion

	#region Properties
	
	internal IntArithmetic(IntValue a_left, EArithmeticOperation a_op, IntValue a_right)
	{
		leftVal = a_left;
		operation = a_op;
		rightVal = a_right;
	}
	#endregion

	#region Methods
	internal override int Value
	{
		get
		{
			int result = 0;
			
			if(leftVal != null && rightVal != null)
			{
				switch(operation)
				{
					case EArithmeticOperation.Plus :
					result = leftVal.Value + rightVal.Value;
					break;
					
					case EArithmeticOperation.Minus :
					result = leftVal.Value - rightVal.Value;
					break;
					
					case EArithmeticOperation.Mult :
					result = leftVal.Value * rightVal.Value;
					break;
					
					case EArithmeticOperation.Div :
					result = leftVal.Value / rightVal.Value;
					break;
					
					case EArithmeticOperation.Mod :
					result = leftVal.Value % rightVal.Value;
					break;
					
					case EArithmeticOperation.Max :
					result = Mathf.Max(leftVal.Value, rightVal.Value);
					break;
					
					case EArithmeticOperation.Min :
					result = Mathf.Min(leftVal.Value, rightVal.Value);
					break;
					
					case EArithmeticOperation.Dist :
					result = Mathf.Abs(leftVal.Value - rightVal.Value);
					break;
				}
			}
			
			return result;
		}
	}
	#endregion
}
