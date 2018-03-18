using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Emulator {
	/// <summary>
	/// Apagar personagem - size 44
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_211 {
		// Atributos
		public SHeader Header;          // 0 a 11		= 12

		public int Slot;                // 12 a 15	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] NameBytes;      // 16 a 27	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] Unk1;           // 28 a 31	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 10 )]
		public byte [ ] PasswordBytes;  // 32 a 41	= 10

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public byte [ ] Unk2;           // 42 a 43	= 2

		// Ajudantes
		public string Name => Functions.GetString ( this.NameBytes );
		public string Password => Functions.GetString ( this.PasswordBytes );

		// Controlador
		public static void Controller ( Client client , P_211 rcv ) {
			if ( !Regex.IsMatch ( rcv.Name , @"^[A-Za-z0-9-]{4,12}$" ) ) {
				client.Close ( "Nome inválido!" );
			} else if ( rcv.Slot < 0 || rcv.Slot > 3 ) {
				client.Close ( );
			} else if ( !Regex.IsMatch ( rcv.Password , @"^[A-Za-z0-9]{4,10}$" ) ) {
				client.Close ( );
			} else {
				// Retorna character da conta
				ref Character character = ref client.Account.Characters [ rcv.Slot ];

				// Verifica se está vaziu
				if ( character == null ) {
					client.Close ( );
				} else {
					// Envia os pacotes de apagar personagem
					client.Send ( P_112.New ( client ) );
					client.Send ( P_101.New ( $"Personagem [{rcv.Name}] deletado!" ) );
				}
			}
		}
	}
}