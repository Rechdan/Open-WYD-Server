using System;
using System.Runtime.InteropServices;

namespace Emulator {
	[StructLayout ( LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1 )]
	public struct SHeader // 12
	{
		public short size;      // 0 a 1	= 2
		public byte key;        // 2			= 1
		public byte checkSum;   // 3			= 1
		public short packetID;  // 4 a 5	= 2
		public short clientID;  // 6 a 7	= 2
		public int timeStamp;   // 8 a 11	= 4

		public static SHeader New ( short packetID, int size, int clientID ) {
			SHeader tmp = new SHeader {
				size = ( short ) ( size ),
				key = 0,
				checkSum = 0,
				packetID = packetID,
				clientID = ( short ) ( clientID ),
				timeStamp = Environment.TickCount
			};

			return tmp;
		}
	}
}