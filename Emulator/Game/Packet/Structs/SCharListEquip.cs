using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Equipamentos do personagem na CharList - size 128
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SCharListEquip {
		// Atributos
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public SItem [ ] Slot;  // 0 a 127	= 128

		// Atributos
		public static SCharListEquip New ( ) {
			SCharListEquip tmp = new SCharListEquip {
				Slot = new SItem [ 16 ]
			};

			for ( int i = 0 ; i < tmp.Slot.Length ; i++ ) {
				tmp.Slot [ i ] = SItem.New ( );
			}

			return tmp;
		}
		public static SCharListEquip New ( SItem [ ] Item ) {
			SCharListEquip tmp = new SCharListEquip ( ) {
				Slot = Item
			};

			return tmp;
		}
	}
}