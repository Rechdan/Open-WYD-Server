using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Entrar no mundo - size 1712
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_114 {
		// Atributos
		public SHeader Header;      // 0 a 11				= 12

		public SPosition Position;  // 12 a 15			= 4

		public SMob Mob;            // 16 a 1351		= 1336

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 360 )]
		public byte [ ] Unk1;         // 1352 a 1711	= 360

		// Construtores
		public static P_114 New ( Character character ) {
			P_114 tmp = new P_114 {
				Header = SHeader.New ( 0x114 , Marshal.SizeOf<P_114> ( ) , 30001 ) ,

				Position = character.Mob.LastPosition ,

				Mob = character.Mob ,

				Unk1 = new byte [ 360 ]
			};

			return tmp;
		}
	}
}