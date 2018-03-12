# Open WYD Server

Wanna talk to the developer? So [enter in our Discord](https://discord.gg/JGqKpr) now!


## Support our work

Be my [Patreon](https://www.patreon.com/Rechdan) and show your support! :)


## Configuration

It's pretty easy, you only need to configure the Initialize method in Config.cs:

	public static void Initialize ( ) {
		Game = new Game ( );
		
		Game
			.AddServer ( new Server ( "First" ), server => {
				server
					.AddChannel ( new Channel ( server, "192.168.50.100" ), null );
			} )
			.AddServer ( new Server ( "Second" ), server => {
				server
					.AddChannel ( new Channel ( server, "127.0.0.1" ), null )
					.AddChannel ( new Channel ( server, "127.0.0.2" ), null );
			} );
		
		Game.Run ( );
	}


## Execution

 1. **Windows**

Install the .NET Framework 4.7.1 and then simply run the .exe file in the **Emulator/Build** folder

 2. **Linux**

Install MonoDevelop, then open the Terminal in the **Emulator/Build** folder and then write **mono Emulator.exe**
