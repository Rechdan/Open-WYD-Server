using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Cabeçalho dos pacotes - size 12
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SHeader {
		// Atributos
		public short Size;      // 0 a 1	= 2
		public byte Key;        // 2			= 1
		public byte CheckSum;   // 3			= 1
		public short PacketId;  // 4 a 5	= 2
		public short ClientId;  // 6 a 7	= 2
		public int TimeStamp;   // 8 a 11	= 4

		// Construtores
		public static SHeader New ( short PacketID , int Size , int ClientID ) {
			SHeader tmp = new SHeader {
				Size = ( short ) ( Size ) ,
				Key = 0 ,
				CheckSum = 0 ,
				PacketId = PacketID ,
				ClientId = ( short ) ( ClientID ) ,
				TimeStamp = Environment.TickCount
			};

			return tmp;
		}
	}
}