using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura dos adicionais do item da ItemList - size 4
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SItemListEF {
		// Atributos
		public short Index; // 0 a 1	= 2
		public short Value; // 2 a 3	= 2
	}
}