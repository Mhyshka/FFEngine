using UnityEngine;
using System.Collections;

public class PlayerOrderScript : APlayerScript
{
	#region Init
	internal override void Init (Player a_player)
	{
		base.Init (a_player);
		RegisterForEvent();
	}
	
	protected virtual void OnDestroy()
	{
		UnregisterForEvent();
	}
	#endregion
	
	
	#region Event Management
	protected virtual void RegisterForEvent ()
	{
		FFEngine.Inputs.RegisterOnEventKey(EInputEventKey.Interaction, OnTryInteraction);
	}
	
	protected virtual void UnregisterForEvent ()
	{
		FFEngine.Inputs.UnregisterOnEventKey(EInputEventKey.Interaction, OnTryInteraction);
	}
	#endregion
	
	#region Selection
	protected virtual void OnTryInteraction()
	{
		if(_player.selection.HoveredObject != null)
		{
			//Try interact
		}
		else if(_player.selection.HasTerrainPosition)
		{
			MoveToPositionOrder order = new MoveToPositionOrder(_player.selection.TerrainPosition,
																MoveToPositionOrder.EType.Move);
			_player.hero.OnMoveOrderReceived(order,
											_player.selection.TerrainPosition);
		}
	}
	#endregion
}
