using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Entrar no mundo - size 36
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_213 {
		// Atributos
		public SHeader Header;  // 0 a 11		= 12

		public int Slot;        // 12 a 15	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 20 )]
		public byte [ ] Unk1;   // 16 a 35	= 20

		// Controlador
		public static void Controller ( Client client , P_213 rcv ) {
			if ( rcv.Slot < 0 || rcv.Slot > 3 ) {
				client.Close ( );
			} else {
				Character character = client.Account.Characters [ rcv.Slot ];

				if ( character == null ) {
					client.Close ( );
				} else {
					short ClientId = client.Channel.GetClientId ( );

					character.Mob.ClientId = ClientId;

					if ( ClientId < Config.Values.Clients.MinCid ) {
						client.Send ( P_101.New ( "Parece que este canal está lotado. Tente novamente!" ) );
					} else {
						client.Send ( P_114.New ( character ) );
						client.Send ( P_364.New ( character , EnterVision.LogIn ) );
						client.Send ( P_101.New ( "Uma mensagem qualquer..." ) );

						client.Status = ClientStatus.Game;
					}
				}
			}
		}
	}
}