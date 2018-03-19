using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	/// <summary>
	/// Andar - size 52
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_36C {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		public SPosition Src;       // 12 a 15	= 4

		public int Type;            // 16 a 19	= 4
		public int Speed;           // 20 a 23	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] Directions;  // 24 a 35	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] Unk1;       // 36 a 47	= 12

		public SPosition Dst;       // 48 a 51	= 4

		// Construtores
		public static P_36C New ( int ClientId , SPosition Src , SPosition Dst , int Type , int Speed , byte [ ] Directions ) {
			P_36C tmp = new P_36C {
				Header = SHeader.New ( 0x36C , Marshal.SizeOf<P_36C> ( ) , ClientId ) ,

				Src = Src ,

				Type = Type ,
				Speed = Speed ,

				Directions = new byte [ 12 ] ,

				Unk1 = new byte [ 12 ] ,

				Dst = Dst ,
			};

			Array.Copy ( Directions , tmp.Directions , tmp.Directions.Length );

			return tmp;
		}

		// Controlador
		public static void Controller ( Client client , P_36C rcv ) {
			// Guarda as direções que o cliente vai andar
			List<byte> directions = new List<byte> ( );
			// Guarda a altura do mapa
			int height = Config.HeightMap ( client.Character.Mob.LastPosition );
			// Guarda a posição do cliente
			Coord cur = client.Map.GetCoord ( client.Character.Mob.LastPosition );

			Log.Normal ( $"[{rcv.Src.X}, {rcv.Src.Y}] > [{rcv.Dst.X}, {rcv.Dst.Y}]" );
			Log.Normal ( string.Join ( ", " , rcv.Directions.Select ( a => a.ToString ( ).PadLeft ( 2 , '0' ) ) ) );
			// Verifica se 
		}
	}
}