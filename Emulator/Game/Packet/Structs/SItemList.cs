using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura da ItemList - size 140
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SItemList {
		// Atributos
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 64 )]
		public byte [ ] NameBytes;  // 0 a 63			= 64

		public short Mesh;          // 64 a 65		= 2
		public int Texture;         // 66 a 69		= 4

		public short Level;         // 70 a 71		= 2
		public short Str;           // 72 a 73		= 2
		public short Int;           // 74 a 75		= 2
		public short Dex;           // 76 a 77		= 2
		public short Con;           // 78 a 79		= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public SItemListEF [ ] Ef;  // 80 a 127		= 48

		public int Price;           // 128 a 131	= 4
		public short Unique;        // 132 a 133	= 2
		public short Slot;          // 134 a 135	= 2
		public short Extreme;       // 136 a 137	= 2
		public short Grade;         // 138 a 139	= 2

		// Ajudantes
		public string Name {
			get => Functions.GetString ( this.NameBytes );
			set {
				this.NameBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.NameBytes , 64 );
			}
		}

		// Operadores
		public static Score operator + ( SItemList Current , SItemEF [ ] Efs ) {
			Score rtn = Score.New ( ) + Current;

			for ( int i = 0 ; i < Efs.Length ; i++ ) {
				short val = Efs [ i ].Value;

				switch ( Efs [ i ].Type ) {
					case 2:
					case 67:
					case 73:
						rtn.Attack += val;
						break;

					case 3:
					case 53:
					case 72:
						rtn.Defense += val;
						break;

					case 4:
						rtn.HP += val;
						break;

					case 5:
						rtn.MP += val;
						break;

					case 7:
						rtn.Str += val;
						break;

					case 8:
						rtn.Int += val;
						break;

					case 9:
						rtn.Dex += val;
						break;

					case 10:
						rtn.Con += val;
						break;

					case 11:
						rtn.Master [ 0 ] += val;
						break;

					case 12:
						rtn.Master [ 1 ] += val;
						break;

					case 13:
						rtn.Master [ 2 ] += val;
						break;

					case 14:
						rtn.Master [ 3 ] += val;
						break;

					case 26:
						rtn.AttackSpeed += val;
						break;

					case 29:
						rtn.MoveSpeed += val;
						break;

					case 33:
						rtn.Grid = ( byte ) ( val );
						break;

					case 42:
					case 71:
						rtn.Critical += val;
						break;

					case 45:
					case 69:
						rtn.PHP += val;
						break;

					case 46:
					case 70:
						rtn.PMP += val;
						break;

					case 47:
						rtn.HPRegen += val;
						break;

					case 48:
						rtn.MPRegen += val;
						break;

					case 49:
						rtn.resistFire += val;
						break;

					case 50:
						rtn.resistIce += val;
						break;

					case 51:
						rtn.resistHoly += val;
						break;

					case 52:
						rtn.resistThunder += val;
						break;

					case 54:
						rtn.resistFire += val;
						rtn.resistIce += val;
						rtn.resistHoly += val;
						rtn.resistThunder += val;
						break;

					case 60:
					case 68:
						rtn.Magic += val;
						break;

					case 74:
						for ( int j = 1 ; j < rtn.Master.Length ; j++ ) {
							rtn.Master [ j ] += val;
						}
						break;
				}
			}

			return rtn;
		}
	}
}