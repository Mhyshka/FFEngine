namespace FF
{
	internal enum EStateID
	{
		None = -1,
		Loading = 1000,
		Tuto,
		Game,
		Result,
		Exit
	}
	
	internal enum EMenuStateID
	{
		None = -1,
		Main = 2000,
		GameModePicker,
		HostList,
		GameRoomHost,
		GameRoomClient,
		Settings
	}
	
	internal enum EGameModeID
	{
		None = -1,
		Menu = 1000,
		Game
	}
	
	public enum EEventType
	{
		Custom = -1,
		UILoadingComplete = 0,
		GMLoadingComplete,
		AsyncLoadingComplete,
		Next,
		Back,
		Confirm,
		Decline,
		BackToMenu
	}
	
	internal enum EInputEventKey
	{
		Select,
		Interaction,
		Spell1,
		Spell2,
		Spell3,
		Spell4,
		Quit
	}
	
	internal enum EInputSwitchKey
	{
		Switch1,
		Switch2
	}
	
	internal enum EInputAxisKey
	{
		Vertical,
		Horizontal,
		Scroll
	}
}