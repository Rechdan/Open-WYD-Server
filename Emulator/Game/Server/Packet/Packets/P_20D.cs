using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Emulator {
	/// <summary>
	/// Login - size 116
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_20D {
		// Atributos
		public SHeader Header;          // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 10 )]
		public byte [ ] PasswordBytes;  // 12 a 21	= 10

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public byte [ ] Unk1;           // 22 a 23	= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] UserNameBytes;  // 24 a 35	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 56 )]
		public byte [ ] Unk2;           // 36 a 91	= 56

		public int Version;             // 92 a 95	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 20 )]
		public byte [ ] Unk3;           // 96 a 115	= 20

		// Ajudantes
		public string UserName => Functions.GetString ( this.UserNameBytes );
		public string Password => Functions.GetString ( this.PasswordBytes );

		// Controlador
		public static void Controller ( Client client , P_20D rcv ) {
			if ( !Regex.IsMatch ( rcv.UserName , @"^[A-Za-z0-9]{4,12}$" ) ) {
				client.Close ( "Somente letras e números no login. 4 a 12 caracteres." );
			} else if ( !Regex.IsMatch ( rcv.Password , @"^[A-Za-z0-9]{4,10}$" ) ) {
				client.Close ( "Somente letras e números na senha. 4 a 10 caracteres." );
			} else {
				Log.Information ( $"UserName: {rcv.UserName}, {rcv.UserName.Length}" );
				Log.Information ( $"Password: {rcv.Password}, {rcv.Password.Length}" );

				P_10A p010A = P_10A.New ( client );

				p010A.UserName = rcv.UserName;

				client.Send ( p010A );
				client.Send ( P_101.New ( "Entre com sua senha numérica!" ) );

				client.Status = ClientStatus.Numeric;
			}
		}
	}
}