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
		public static void Controller ( Client client, byte [ ] data ) {
			lock ( Lock ) {
				SHeader header = PConvert.ToStruct<SHeader> ( data );

				Log.Rcv ( client, header );

				switch ( client.Status ) {
					case ClientStatus.Login: {
						switch ( header.packetID ) {
							case 0x20D: break;  // Login

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Numeric: {
						switch ( header.packetID ) {
							case 0xFDE: break;  // Senha numérica

							default: client.Close ( ); break;
						}

						break;
					}

					case ClientStatus.Characters: {
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