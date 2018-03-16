using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Criar personagem - size 856
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_110 {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		public SCharList CharList;  // 12 a 855	= 844

		// Construtores
		public static P_110 New ( Client client ) {
			P_110 tmp = new P_110 {
				Header = SHeader.New ( 0x110 , Marshal.SizeOf<P_110> ( ) , 30001 ) ,
				CharList = SCharList.New ( client )
			};

			return tmp;
		}
	}
}