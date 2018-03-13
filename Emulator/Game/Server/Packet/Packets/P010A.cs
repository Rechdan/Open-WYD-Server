using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Tela de seleção de personagens - size 1928
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P010A {
		// Atributos
		public SHeader Header;          // 0 a 11				= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] Unk1;           // 12 a 27			= 16

		public SCharList CharList;      // 28 a 871			= 844

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 120 )]
		public SItem [ ] Cargo;         // 872 a 1831		= 960

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 64 )]
		public byte [ ] Unk2;           // 1832 a 1895	= 64

		public int Gold;                // 1896 a 1899	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] UserNameBytes;  // 1900 a 1911	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] Unk3;           // 1912 a 1927	= 16

		// Ajudantes
		public string UserName {
			get => Functions.GetString ( this.UserNameBytes );
			set {
				this.UserNameBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.UserNameBytes , 12 );
			}
		}

		// Construtores
		public static P010A New ( Client client ) {
			P010A tmp = new P010A {
				Header = SHeader.New ( 0x010A , Marshal.SizeOf<P010A> ( ) , 30002 ) ,

				Unk1 = new byte [ 16 ] ,

				CharList = SCharList.New ( client ) ,
				Cargo = new SItem [ 120 ] ,

				Unk2 = new byte [ 64 ] ,

				Gold = 0 ,
				UserName = "" ,

				Unk3 = new byte [ 16 ]
			};

			return tmp;
		}
	}
}