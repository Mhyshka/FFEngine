using UnityEngine;
using System.Collections;

public class UnitLife : AUnitComponent
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
			return FFEngine.Game.Constants.STRENGTH_HP_PER_POINT * _unit.stats.strength.Value;
		}
	}
	
	protected int _current;
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		
		/*life.bonusIsFlatFirst = FFEngine.Game.Constants.LIFE_BONUS_IS_FLAT_FIRST;
		life.malusIsFlatFirst = FFEngine.Game.Constants.LIFE_MALUS_IS_FLAT_FIRST;*/
		
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
				if(_unit.onDeath != null)
					_unit.onDeath();
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
	
	#region Events
	internal override void RegisterForEvents ()
	{
		base.RegisterForEvents ();
		_unit.onStatsModification += OnStatsModification;
	}
	
	protected override void UnregisterForEvents ()
	{
		base.UnregisterForEvents ();
		_unit.onStatsModification -= OnStatsModification;
	}
	
	internal void OnStatsModification()
	{
	
	}
	#endregion
}
