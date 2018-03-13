using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Alerta - size 108
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P0101 {
		// Atributos
		public SHeader Header;        // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 80 )]
		public byte [ ] MessageBytes; // 12 a 91	= 80

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] Unk1;         // 92 a 107	= 16

		// Ajudantes
		public string Message {
			get => Functions.GetString ( this.MessageBytes );
			set {
				this.MessageBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.MessageBytes , 80 );
			}
		}

		// Construtores
		public static P0101 New ( string Message ) {
			P0101 tmp = new P0101 {
				Header = SHeader.New ( 0x0101 , Marshal.SizeOf<P0101> ( ) , 0 ) ,
				Message = Message ,
				Unk1 = new byte [ 16 ]
			};

			return tmp;
		}
	}
}