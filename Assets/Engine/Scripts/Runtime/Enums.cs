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
		ModeSelection,
		SearchForRooms,
		GameRoomHost,
		GameRoomClient,
		WifiCheck,
        Connecting,
		Settings
	}
	
	internal enum EGameModeID
	{
		None = -1,
		Menu = 1000,
		Game
	}
	
	public enum FFEventType
	{
		Custom = -1,
		Next = 0,
		Back,
		Confirm,
		Decline,
		BackToMenu,
		Connect,
        PlayAgain
	}
	
	internal enum EInputEventKey
	{
        Action,
        Up,
        Down,
        Left,
        Right,
        Submit,
        Back

        /*Select,
		Interaction,
		Spell1,
		Spell2,
		Spell3,
		Spell4,
		Quit*/
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