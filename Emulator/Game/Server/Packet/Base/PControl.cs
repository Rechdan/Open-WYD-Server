using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
							case 0x20D: P020D.Controller ( client , PConvert.ToStruct<P020D> ( data ) ); break;  // Login

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Numeric: {
						switch ( header.PacketID ) {
							case 0xFDE: P0FDE.Controller ( client , PConvert.ToStruct<P0FDE> ( data ) ); break;  // Senha numérica

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Characters: {
						switch ( header.PacketID ) {
							case 0x020F: break; // Criar personagem
							case 0x0211: break; // Apagar personagem
							case 0x0213: break; // Entrar no mundo

							case 0xFDE: P0FDE.Controller ( client , PConvert.ToStruct<P0FDE> ( data ) ); break;  // Alterar senha numérica

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Game: {
						break;
					}
				}
			}
		}
	}
}