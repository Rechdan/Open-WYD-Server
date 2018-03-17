using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Emulator {
	public static class Config {
		// Atributos
		public static DateTime Time => DateTime.UtcNow;

		public static readonly CultureInfo Culture = new CultureInfo ( "pt-BR" );
		public static readonly Encoding Encoding = Encoding.GetEncoding ( "Windows-1252" );

		public static readonly Random Random = new Random ( ( int ) ( Time.Ticks ) );

		public static readonly string Dir = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}";

		public static Game Game { get; private set; }

		public static sbyte [ , ] Map { get; private set; }

		// Inicializador
		public static void Initialize ( ) {
			Console.Title = "Open WYD Server";

			Log.Line ( );

			LoadMap ( );

			Log.Line ( );

			Game = new Game ( );

			Game
				.AddServer ( new Server ( "Name 1" ) , server => {
					server
						.AddChannel ( new Channel ( server , "192.168.50.100" ) , null );
				} )
				.AddServer ( new Server ( "Name 2" ) , server => {
					server
						.AddChannel ( new Channel ( server , "127.0.0.1" ) , null )
						.AddChannel ( new Channel ( server , "127.0.0.2" ) , null );
				} )
				.Run ( );
		}

		// Valores
		public static class Values {
			public static class Clients {
				// Define o client de 1 a 900 totalizando 899 por canal
				public const int MinCid = 1, MaxCid = 900;
			}

			public static class Field {
				// Limite do campo de visão
				public const int View = 22;
			}
		}

		// Carrega o mapa
		public static void LoadMap ( ) {
			bool update = ( Map != null );

			DateTime start = Time;

			Map = new sbyte [ 4096 , 4096 ];

			byte [ ] load = File.ReadAllBytes ( $"{Dir}HeightMap.dat" );

			for ( int y = 0 ; y < 4096 ; y++ ) {
				for ( int x = 0 ; x < 4096 ; x++ ) {
					Map [ x , y ] = ( sbyte ) ( load [ ( x + y * 4096 ) ] );
				}
			}

			/*List<sbyte [ ]> print = new List<sbyte [ ]> ( );
			for ( int x = -25 ; x <= 25 ; x++ ) {
				List<sbyte> line = new List<sbyte> ( );
				for ( int y = -25 ; y <= 25 ; y++ ) {
					line.Add ( Map [ 2100 + x , 2100 + y ] );
				}
				print.Add ( line.ToArray ( ) );
			}
			Log.Normal ( $"Test:\n{string.Join ( "\n" , print.Select ( a => string.Join ( "," , a.Select ( b => $"{b}".PadLeft ( 4 , ' ' ) ) ) ) )}" );*/

			//Log.Normal ( t & ( 1 << 10 ) );

			Log.Information ( $"Mapa carregado em [{Time - start}]" );
		}
	}
}