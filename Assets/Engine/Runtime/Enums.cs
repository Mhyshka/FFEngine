namespace FFEngine
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
		Title = 2000,
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
		UILoadingComplete,
		GMLoadingComplete,
		AsyncLoadingComplete,
		Next,
		Back,
		NextState,
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