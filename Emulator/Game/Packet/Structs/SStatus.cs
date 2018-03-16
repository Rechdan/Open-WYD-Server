using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Status dos mobs - size 48
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SStatus {
		// Atributos
		public int Level;         // 0 a 3		= 4

		public int Defense;       // 4 a 7		= 4
		public int Attack;        // 8 a 11		= 4

		public byte Merchant;     // 12				= 1
		public byte MobSpeed;     // 13				= 1
		public byte Direction;    // 14				= 1
		public byte ChaosRate;    // 15				= 1

		public uint MaxHP;        // 16 a 19	= 4
		public uint MaxMP;        // 20 a 23	= 4
		public uint CurHP;        // 24 a 27	= 4
		public uint CurMP;        // 28 a 31	= 4

		public short Str;         // 32 a 33	= 2
		public short Int;         // 34 a 35	= 2
		public short Dex;         // 36 a 37	= 2
		public short Con;         // 38 a 39	= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public short [ ] Master;  // 40 a 47	= 8

		// Construtores
		public static SStatus New ( ) {
			SStatus tmp = new SStatus ( ) {
				Level = 0 ,

				Defense = 0 ,
				Attack = 0 ,

				Merchant = 0 ,
				MobSpeed = 0 ,
				Direction = 0 ,
				ChaosRate = 0 ,

				MaxHP = 0 ,
				MaxMP = 0 ,
				CurHP = 0 ,
				CurMP = 0 ,

				Str = 0 ,
				Int = 0 ,
				Dex = 0 ,
				Con = 0 ,

				Master = new short [ 4 ] { 0 , 0 , 0 , 0 }
			};

			return tmp;
		}
		public static SStatus New ( SStatus o ) {
			SStatus tmp = new SStatus ( ) {
				Level = o.Level ,

				Defense = o.Defense ,
				Attack = o.Attack ,

				Merchant = o.Merchant ,
				MobSpeed = o.MobSpeed ,
				Direction = o.Direction ,
				ChaosRate = o.ChaosRate ,

				MaxHP = o.MaxHP ,
				MaxMP = o.MaxMP ,
				CurHP = o.CurHP ,
				CurMP = o.CurMP ,

				Str = o.Str ,
				Int = o.Int ,
				Dex = o.Dex ,
				Con = o.Con ,

				Master = o.Master.ToArray ( )
			};

			return tmp;
		}
	}
}