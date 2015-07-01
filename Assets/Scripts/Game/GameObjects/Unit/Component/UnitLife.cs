using UnityEngine;
using System.Collections;

public class UnitLife : AUnitComponent, IStatsModificationCallbacks
{	
	#region Inspector Properties
	public IntModified life = new IntModified();
	
	[Range(0f,1f)]
	public float startPercentage = 1f;
	#endregion

	#region Properties
	internal int Base
	{
		get
		{
			return life.baseValue;
		}
	}
	
	internal int Max
	{
		get
		{
			return life.Value ;
		}
	}
	
	internal int Current
	{
		get
		{
			return _current;
		}
	}
	
	internal int BonusMaxHP
	{
		get
		{
			return GameConstants.STRENGTH_HP_PER_POINT * _unit.stats.strength.Value;
		}
	}
	
	protected int _current;
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		life.isFlatFirst = GameConstants.LIFE_BONUS_HP_IS_FLAT_FIRST;
		
		OnStatsModification();
		_current = Mathf.Clamp(Mathf.CeilToInt(Max * startPercentage), 
								1,
								Max);
	}
	
	
	internal void Damage(int a_amount)
	{
		if(a_amount > 0)
		{
			//Debug.Log("Damage received : " + a_amount);
			_current -= a_amount;
			if(_current <= 0)
			{
				_unit.OnDeath();
			}
		}
		else if(a_amount == 0)
		{
			Debug.LogWarning("Zero damage received.");
		}
		else
		{
			Debug.LogWarning("Negative damage received : " + a_amount);
		}
	}
	#endregion
	
	#region Stats Callbacks
	public void OnStatsModification()
	{
		life.modifier.flat = BonusMaxHP;
	}
	#endregion
}
