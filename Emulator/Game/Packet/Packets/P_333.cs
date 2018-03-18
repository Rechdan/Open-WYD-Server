using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Chat aberto - size 140
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_333 {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 64 )]
		public byte [ ] TextBytes;  // 12 a 75	= 64

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 64 )]
		public byte [ ] Unk1;       // 76 a 139	= 64

		// Ajudantes
		public string Text {
			get => Functions.GetString ( this.TextBytes );
			set {
				this.TextBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.TextBytes , 64 );
			}
		}

		// Construtores
		public static P_333 New ( int ClientId , string Text ) {
			P_333 tmp = new P_333 ( ) {
				Header = SHeader.New ( 0x0333 , Marshal.SizeOf<P_333> ( ) , ClientId ) ,

				Text = Text ,

				Unk1 = new byte [ 82 ]
			};

			return tmp;
		}

		// Controlador
		public static void Controller ( Client client , P_333 rcv ) {
			P_333 p333 = New ( client.ClientId , rcv.Text );

			client.Surround.GetSurrounds ( ).ForEach ( a => {
				switch ( a ) {
					case Client client2: {
						client2.Send ( p333 );
						break;
					}
				}
			} );
		}
	}
}