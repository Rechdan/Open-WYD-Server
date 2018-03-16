using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura do affect - size 8
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SAffect {
		// Atributos
		public byte index;    // 0			= 1
		public byte master;   // 1			= 1
		public ushort value;  // 2 a 3	= 2
		public uint time;     // 4 a 7	= 4

		// Construtores
		public static SAffect New ( ) {
			SAffect tmp = new SAffect {
				index = 0 ,
				master = 0 ,
				value = 0 ,
				time = 0
			};

			return tmp;
		}
		public static SAffect New ( SAffect other ) {
			SAffect tmp = new SAffect {
				index = other.index ,
				master = other.master ,
				value = other.value ,
				time = other.time
			};

			return tmp;
		}
		public static SAffect New ( byte index , byte master , ushort value , uint time ) {
			SAffect tmp = new SAffect {
				index = index ,
				master = master ,
				value = value ,
				time = time
			};

			return tmp;
		}
	}
}