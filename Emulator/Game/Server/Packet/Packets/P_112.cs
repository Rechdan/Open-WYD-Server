using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Apagar personagem - size 856
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_112 {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		public SCharList CharList;  // 12 a 855	= 844

		// Construtores
		public static P_112 New ( Client client ) {
			P_112 tmp = new P_112 {
				Header = SHeader.New ( 0x112 , Marshal.SizeOf<P_112> ( ) , 30001 ) ,
				CharList = SCharList.New ( client )
			};

			return tmp;
		}
	}
}