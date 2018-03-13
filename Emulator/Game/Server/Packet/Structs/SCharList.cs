using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Tela de personagens - size 844
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SCharList {
		// Atributos
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] Unk1;             // 0 a 3			= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public short [ ] PosX;            // 4 a 11			= 8

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public short [ ] PosY;            // 12 a 19		= 8

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public SCharListName [ ] Name;    // 20 a 83		= 64

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public SStatus [ ] Status;        // 84 a 275		= 192

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public SCharListEquip [ ] Equips; // 276 a 787	= 512

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 8 )]
		public byte [ ] Unk2;             // 788 a 795	= 8

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public int [ ] Gold;              // 796 a 811	= 16

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public ulong [ ] Exp;             // 812 a 843	= 32

		// Construtores
		public static SCharList New ( Client client ) {
			SCharList tmp = new SCharList {
				Unk1 = new byte [ 4 ] { 0 , 0 , 0 , 0 } ,

				PosX = new short [ 4 ] ,
				PosY = new short [ 4 ] ,

				Name = new SCharListName [ 4 ] ,
				Status = new SStatus [ 4 ] ,
				Equips = new SCharListEquip [ 4 ] ,

				Unk2 = new byte [ 8 ] { 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 } ,

				Gold = new int [ 4 ] ,
				Exp = new ulong [ 4 ]
			};

			for ( int i = 0 ; i < 4 ; i++ ) {
				tmp.PosX [ i ] = 0;
				tmp.PosY [ i ] = 0;

				tmp.Name [ i ] = SCharListName.New ( "" );
				tmp.Status [ i ] = SStatus.New ( );
				tmp.Equips [ i ] = SCharListEquip.New ( );

				tmp.Gold [ i ] = 0;
				tmp.Exp [ i ] = 0;
			}

			return tmp;
		}
	}
}