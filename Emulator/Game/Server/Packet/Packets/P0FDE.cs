using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Emulator {
	/// <summary>
	/// Senha numérica - size 32
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P0FDE {
		// Atributos
		public SHeader Header;        // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 6 )]
		public byte [ ] NumericBytes; // 12 a 17	= 6

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 14 )]
		public byte [ ] Unk1;         // 18 a 31	= 14

		// Ajudantes
		public string Numeric => Functions.GetString ( this.NumericBytes );

		// Controlador
		public static void Controller ( Client client , P0FDE rcv ) {
			if ( !Regex.IsMatch ( rcv.Numeric , @"^[0-9]{4,6}$" ) ) {
				client.Close ( "Senha numérica inválida!" );
			} else {
				Log.Information ( $"Numeric: {rcv.Numeric}, {rcv.Numeric.Length}" );

				client.Status = ClientStatus.Characters;
			}
		}
	}
}