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

			Game.AddServer ( new Server ( "Destiny" ), server => {
				server.AddChannel ( new Channel ( server, "192.168.50.100" ), channel => { } );
			} );

			Game.Run ( );
		}
	}
}