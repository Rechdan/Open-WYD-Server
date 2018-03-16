using System;

namespace Emulator {
	public static class Log {
		// Atributos
		private static readonly string Lock = "";

		// Métodos
		public static void Write ( object o , ConsoleColor c ) {
			lock ( Lock ) {
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write ( $"[{Config.Time:dd/MM/yyyy HH:mm:ss.fffff}]: " );

				Console.ForegroundColor = c;
				Console.WriteLine ( $"{o}" );

				Console.ResetColor ( );
			}
		}

		public static void Normal ( object o ) => Write ( o , ConsoleColor.White );
		public static void Information ( object o ) => Write ( o , ConsoleColor.Cyan );
		public static void Error ( object o ) => Write ( o , ConsoleColor.Red );

		public static void Line ( ) => Console.WriteLine ( );

		public static void Conn ( Client c , bool i ) => Write ( $"Cliente {c.Socket.RemoteEndPoint} se {( i ? "" : "des" )}conectou" , ConsoleColor.Yellow );

		public static void Rcv ( Client c , SHeader h ) => Write ( $"RCV > P: 0x{h.PacketID.ToString ( "X" ).PadLeft ( 4 , '0' )} | S: {h.Size.ToString ( ).PadLeft ( 4 , '0' )} | CID: {h.ClientID.ToString ( ).PadLeft ( 5 , '0' )}" , ConsoleColor.Magenta );
		public static void Snd ( Client c , SHeader h ) => Write ( $"SND > P: 0x{h.PacketID.ToString ( "X" ).PadLeft ( 4 , '0' )} | S: {h.Size.ToString ( ).PadLeft ( 4 , '0' )} | CID: {h.ClientID.ToString ( ).PadLeft ( 5 , '0' )}" , ConsoleColor.Green );
	}
}