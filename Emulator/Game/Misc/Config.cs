using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public static class Config {
		// Atributos
		public static DateTime Time => DateTime.UtcNow;

		public static Game Game { get; private set; }

		// Inicializador
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
	}
}