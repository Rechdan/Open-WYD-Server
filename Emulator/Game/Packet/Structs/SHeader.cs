using System;
using System.Runtime.InteropServices;

namespace Emulator {
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SHeader // 12
	{
		public short Size;      // 0 a 1	= 2
		public byte Key;        // 2			= 1
		public byte CheckSum;   // 3			= 1
		public short PacketID;  // 4 a 5	= 2
		public short ClientID;  // 6 a 7	= 2
		public int TimeStamp;   // 8 a 11	= 4

		public static SHeader New ( short PacketID , int Size , int ClientID ) {
			SHeader tmp = new SHeader {
				Size = ( short ) ( Size ) ,
				Key = 0 ,
				CheckSum = 0 ,
				PacketID = PacketID ,
				ClientID = ( short ) ( ClientID ) ,
				TimeStamp = Environment.TickCount
			};

			return tmp;
		}
	}
}