using System.Runtime.InteropServices;
using System.Text;

namespace Emulator {
	/// <summary>
	/// Login
	/// </summary>
	[StructLayout ( LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1 )]
	public struct P_20D {
		// Atributos
		public SHeader Header;          // 0 a 11		= 12

		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 12 )]
		public byte [ ] PasswordBytes;  // 12 a 23	= 12

		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 16 )]
		public byte [ ] UsernameBytes;  // 24 a 39	= 16

		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 52 )]
		public byte [ ] Unk1;           // 40 a 91	= 52

		public int Version;             // 92 a 95	= 4

		[MarshalAs ( UnmanagedType.ByValArray, SizeConst = 20 )]
		public byte [ ] Unk2;           // 96 a 115	= 20

		// Ajudantes
		public string UserName => Functions.GetString ( this.UsernameBytes );
		public string Password => Functions.GetString ( this.PasswordBytes );

		// Controlador
		public static void Controller ( Client client, P_20D rcv ) {
			Log.Information ( $"UserName: {rcv.UserName}, {rcv.UserName.Length}" );
			Log.Information ( $"Password: {rcv.Password}, {rcv.Password.Length}" );
		}
	}
}