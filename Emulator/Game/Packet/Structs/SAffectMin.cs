using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura do affect da visão - size 2
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SAffectMin {
		// Atributos
		public byte Time;   // 0
		public byte Index;  // 1

		// Construtores
		public static SAffectMin New ( ) {
			SAffectMin tmp = new SAffectMin {
				Time = 0 ,
				Index = 0
			};

			return tmp;
		}
		public static SAffectMin New ( SAffect Affect ) {
			SAffectMin tmp = new SAffectMin {
				Time = 255 ,
				Index = ( byte ) ( Affect.index )
			};

			return tmp;
		}
	}
}