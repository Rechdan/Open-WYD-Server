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
				// Retorna o personagem selecionado
				Character character = client.Account.Characters [ rcv.Slot ];

				if ( character == null ) {
					client.Close ( );
				} else {
					// Retorna um ClientId vaziu
					short ClientId = client.Channel.GetClientId ( );

					if ( ClientId < Config.Values.Clients.MinCid ) {
						client.Send ( P_101.New ( "Parece que este canal está lotado. Tente novamente!" ) );
					} else {
						// Retorna a posição de respawn
						Coord coord = Functions.GetFreeRespawnCoord ( client.Channel.Map , character );

						if ( coord == null ) {
							client.Send ( P_101.New ( "Parece que este o mapa está lotado. Tente novamente!" ) );
						} else {
							// Define o ClientId do cliente
							client.ClientId = character.Mob.ClientId = ClientId;

							// Define a posição do mob
							character.Mob.LastPosition = SPosition.New ( coord );

							// Define o cliente na coordenada
							coord.Client = client;

							// Envia o pacote de entrar no mundo
							client.Send ( P_114.New ( character ) );

							// Prepara o pacote de visão do cliente
							P_364 p364 = P_364.New ( character , EnterVision.LogIn );

							// Envia o pacote de visão do cliente pro cliente
							client.Send ( p364 );

							// Altera o status do cliente
							client.Status = ClientStatus.Game;

							// Define o mapa, character do cliente e inicia o Surround
							client.Map = client.Channel.Map;
							client.Character = character;
							client.Surround = new Surround ( client );

							// Atualiza os arredores e envia a visão para todos e para o cliente
							client.Surround.UpdateSurrounds ( null , entered => {
								entered.ForEach ( a => {
									switch ( a ) {
										case Client client2: {
											client.Send ( P_364.New ( client2.Character , EnterVision.Normal ) );
											client2.Send ( p364 );
											break;
										}
									}
								} );
							} , null );

							// Envia uma mensagem pro cliente
							client.Send ( P_101.New ( "Seja bem-vindo ao mundo do Open WYD Server!" ) );
						}
					}
				}
			}
		}
	}
}