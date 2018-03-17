using System.Collections.Generic;
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

					if ( ClientId < Config.Values.Clients.MinCid ) {
						client.Send ( P_101.New ( "Parece que este canal está lotado. Tente novamente!" ) );
					} else {
						Coord coord = Functions.GetFreeRespawnCoord ( client.Channel.Map , character );

						if ( coord == null ) {
							client.Send ( P_101.New ( "Parece que este o mapa está lotado. Tente novamente!" ) );
						} else {
							client.ClientId = character.Mob.ClientId = ClientId;

							character.Mob.LastPosition = SPosition.New ( coord );

							client.Send ( P_114.New ( character ) );

							P_364 p364 = P_364.New ( character , EnterVision.LogIn );

							client.Send ( p364 );

							client.Status = ClientStatus.Game;

							client.Map = client.Channel.Map;
							client.Character = character;
							client.Surround = new Surround ( client );

							List<object> surrounds = client.Surround.UpdateSurrounds ( );

							surrounds.ForEach ( a => {
								switch ( a ) {
									case Client client2: {
										client.Send ( P_364.New ( client2.Character , EnterVision.Normal ) );
										client2.Send ( p364 );
										break;
									}
								}
							} );

							client.Send ( P_101.New ( "Seja bem-vindo ao mundo do Open WYD Server!" ) );

							client.Surround.SetSurrounds ( surrounds );

							coord.Client = client;

							return;
						}
					}
				}
			}
		}
	}
}