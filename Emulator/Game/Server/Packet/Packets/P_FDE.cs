using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Emulator {
	/// <summary>
	/// Senha numérica - size 32
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_FDE {
		// Atributos
		public SHeader Header;        // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 6 )]
		public byte [ ] NumericBytes; // 12 a 17	= 6

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 10 )]
		public byte [ ] Unk1;         // 18 a 27	= 10

		public int Change;            // 28 a 31	= 4

		// Ajudantes
		public string Numeric => Functions.GetString ( this.NumericBytes );

		// Controlador
		public static void Controller ( Client client , P_FDE rcv ) {
			if ( !Regex.IsMatch ( rcv.Numeric , @"^[0-9]{4,6}$" ) ) {
				client.Close ( "Senha numérica inválida!" );
			} else {
				Log.Information ( $"Numeric: {rcv.Numeric}, {rcv.Numeric.Length}" );

				if ( client.Status == ClientStatus.Numeric ) {
					if ( rcv.Numeric != "25261" ) {
						client.Send ( SHeader.New ( 0x0FDF , Marshal.SizeOf<SHeader> ( ) , 0 ) );
						client.Send ( P_101.New ( "Senha numérica inválida! [25261]" ) );
					} else {
						client.Send ( SHeader.New ( 0x0FDE , Marshal.SizeOf<SHeader> ( ) , 0 ) );
						client.Send ( P_101.New ( "Seja bem-vindo ao Open WYD Server!" ) );

						client.Status = ClientStatus.Characters;
					}
				} else if ( client.Status == ClientStatus.Characters ) {
					if ( rcv.Change != 1 ) {
						client.Close ( );
					} else {
						// Alterar a senha numérica
					}
				} else {
					client.Close ( );
				}
			}
		}
	}
}