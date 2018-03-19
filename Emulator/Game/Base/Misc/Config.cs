using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
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

		private static sbyte [ , ] _HeightMap { get; set; }

		public static SItemList [ ] Itemlist { get; private set; }

		// Inicializador
		public static void Initialize ( ) {
			Console.Title = "Open WYD Server";

			LoadGameFiles ( );

			Game = new Game ( );

			Game
				.AddServer ( new Server ( "Destiny" ) , server => {
					server
						.AddChannel ( new Channel ( server , "192.168.50.100" ) , null );
				} )
				.AddServer ( new Server ( "Tests" ) , server => {
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

		// Métodos
		public static sbyte HeightMap ( SPosition P ) => _HeightMap [ P.X , P.Y ];
		public static sbyte HeightMap ( Coord C ) => _HeightMap [ C.X , C.Y ];
		public static sbyte HeightMap ( int X , int Y ) => _HeightMap [ X , Y ];

		// Método de load de arquivos
		private static void LoadGameFiles ( ) {
			try {
				Log.Line ( );

				LoadHeightMap ( );

				LoadItemlist ( );

				ReadItemName ( );

				Log.Line ( );
			}
			catch ( Exception ex ) {
				Log.Error ( ex );
				Console.ReadKey ( true );
				Environment.Exit ( 0 );
			}
		}

		// Carrega o HeightMap.dat
		public static void LoadHeightMap ( ) {
			bool update = ( _HeightMap != null );

			DateTime start = Time;

			_HeightMap = new sbyte [ 4096 , 4096 ];

			byte [ ] load = File.ReadAllBytes ( $"{Dir}HeightMap.dat" );

			for ( int y = 0 ; y < 4096 ; y++ ) {
				for ( int x = 0 ; x < 4096 ; x++ ) {
					_HeightMap [ x , y ] = ( sbyte ) ( load [ ( x + y * 4096 ) ] );
				}
			}

			load = null;

			Log.Information ( $"HeightMap carregado em [{Time - start}]" );
		}

		// Carrega a ItemList.bin
		public static void LoadItemlist ( ) {
			DateTime start = Time;

			byte [ ] read = File.ReadAllBytes ( $"{Dir}ItemList.bin" );
			int size = read.Length / Marshal.SizeOf<SItemList> ( );

			Itemlist = new SItemList [ size ];

			for ( int i = 0 ; i < read.Length ; i++ ) {
				read [ i ] ^= 0x5A;
			}

			for ( int i = 0 ; i < Itemlist.Length ; i++ ) {
				Itemlist [ i ] = PConvert.ToStruct<SItemList> ( read , i * 140 );
			}

			read = null;

			Log.Information ( $"ItemList, com {size:N0} itens, carregada em [{Time - start}]" );
		}

		// Lê a Itemname.bin para atualizar os nomes na ItemList
		public static void ReadItemName ( ) {
			DateTime start = Time;

			byte [ ] read = File.ReadAllBytes ( $"{Dir}Itemname.bin" );
			int size = ( int ) ( Math.Floor ( read.Length / 68f ) );

			for ( int i = 0 ; i < read.Length ; i += 68 ) {
				for ( int j = i + 4, k = 0 ; j < i + 68 ; j++, k++ ) {
					read [ j ] -= ( byte ) ( k );
				}
			}

			for ( int i = 0, id = 0 ; i < read.Length ; i += 68 ) {
				id = BitConverter.ToInt32 ( read , i );

				Itemlist [ id ].Name = Encoding.Default.GetString ( read , i + 4 , 62 ).Replace ( "\0" , "" );
			}

			read = null;

			Log.Information ( $"Itemname, com {size:N0} nomes, carregada em [{Time - start}]" );
		}
	}
}