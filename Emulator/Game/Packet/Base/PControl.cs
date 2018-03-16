namespace Emulator {
	public static class PControl {
		// Atributos
		private static readonly string Lock = "";

		// Controlador de pacotes
		public static void Controller ( Client client , byte [ ] data ) {
			lock ( Lock ) {
				SHeader header = PConvert.ToStruct<SHeader> ( data );

				Log.Rcv ( client , header );

				if ( header.PacketID == 0x03A0 ) {
					if ( header.Size != 12 || data.Length != 12 ) {
						client.Close ( );
					}

					return;
				}

				switch ( client.Status ) {
					case ClientStatus.Login: {
						switch ( header.PacketID ) {
							case 0x20D: P_20D.Controller ( client , PConvert.ToStruct<P_20D> ( data ) ); break;  // Login

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Numeric: {
						switch ( header.PacketID ) {
							case 0xFDE: P_FDE.Controller ( client , PConvert.ToStruct<P_FDE> ( data ) ); break;  // Senha numérica

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Characters: {
						switch ( header.PacketID ) {
							case 0x020F: P_20F.Controller ( client , PConvert.ToStruct<P_20F> ( data ) ); break; // Criar personagem
							case 0x0211: P_211.Controller ( client , PConvert.ToStruct<P_211> ( data ) ); break; // Apagar personagem
							case 0x0213: P_213.Controller ( client , PConvert.ToStruct<P_213> ( data ) ); break; // Entrar no mundo

							case 0xFDE: P_FDE.Controller ( client , PConvert.ToStruct<P_FDE> ( data ) ); break;  // Alterar senha numérica

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Game: {
						switch ( header.PacketID ) {
							case 0x0291: break; // Depois que entra no mundo
							case 0x03AE: break; // 5 segundos

							default: client.Send ( P_101.New ( $"UNK: 0x{header.PacketID.ToString ( "X" ).PadLeft ( 4 , '0' )}" ) ); break;
						}

						break;
					}
				}
			}
		}
	}
}