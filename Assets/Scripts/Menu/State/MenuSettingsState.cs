using UnityEngine;
using System.Collections;

internal class MenuSettingsState : AMenuGameState
{
	#region Inspector Properties
	#endregion

	#region Properties
	MenuInputPanel panel;
	#endregion

	#region States Methods
	internal override int ID
	{
		get
		{
			return (int)EMenuStateID.Settings;
		}
	}

	internal override void Enter ()
	{
		base.Enter ();
		panel = FFEngine.UI.GetPanel("MenuInputPanel") as MenuInputPanel;
	}

	internal override int Manage ()
	{
		return base.Manage ();
	}

	internal override void Exit ()
	{
		base.Exit ();
	}
	#endregion

	#region Event Management
	protected override void RegisterForEvent ()
	{
		base.RegisterForEvent ();
		FFEngine.Events.RegisterForEvent("RequestKeyPoll", OnRequestKeyPoll);
		FFEngine.Events.RegisterForEvent("RequestAxisPoll", OnRequestAxisPoll);
		FFEngine.Events.RegisterForEvent("InputKeyDetected", OnInputDetected);
		FFEngine.Events.RegisterForEvent("InputAxisDetected", OnInputDetected);
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
		FFEngine.Events.UnregisterForEvent("RequestKeyPoll", OnRequestKeyPoll);
		FFEngine.Events.UnregisterForEvent("RequestAxisPoll", OnRequestAxisPoll);
		FFEngine.Events.UnregisterForEvent("InputKeyDetected", OnInputDetected);
		FFEngine.Events.UnregisterForEvent("InputAxisDetected", OnInputDetected);
	}
	
	internal void OnInputDetected(FFEventParameter a_args)
	{
		panel.HidePoll();
	}

	internal void OnRequestKeyPoll(FFEventParameter a_args)
	{
		InputBindingWidgetKey widget = a_args.data as InputBindingWidgetKey;
		
		panel.DisplayKeyPoll(widget.key.ToString());
		FFEngine.Inputs.StartKeyPoll();
	}
	
	internal void OnRequestAxisPoll(FFEventParameter a_args)
	{
		panel.DisplayKeyPoll("Axis");
		FFEngine.Inputs.StartAxisPoll();
	}
	#endregion

}
