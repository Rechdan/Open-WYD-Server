using System;
using System.Globalization;
using System.Text;

namespace Emulator {
	public static class Config {
		// Atributos
		public static DateTime Time => DateTime.UtcNow;

		public static readonly CultureInfo Culture = new CultureInfo ( "pt-BR" );
		public static readonly Encoding Encoding = Encoding.GetEncoding ( "Windows-1252" );

		public static readonly Random Random = new Random ( ( int ) ( Time.Ticks ) );

		public static Game Game { get; private set; }

		// Inicializador
		public static void Initialize ( ) {
			Console.Title = "Open WYD Server";

			Game = new Game ( );

			Game
				.AddServer ( new Server ( "First" ) , server => {
					server
						.AddChannel ( new Channel ( server , "192.168.50.100" ) , null );
				} )
				.AddServer ( new Server ( "Second" ) , server => {
					server
						.AddChannel ( new Channel ( server , "127.0.0.1" ) , null )
						.AddChannel ( new Channel ( server , "127.0.0.2" ) , null );
				} );

			Game.Run ( );
		}

		// Valores
		public static class Values {
			public static class Clients {
				// Define o client de 1 a 900 totalizando 899 por canal
				public const int MinCid = 1, MaxCid = 900;
			}
		}
	}
}