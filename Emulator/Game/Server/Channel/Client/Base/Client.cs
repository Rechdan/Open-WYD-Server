using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Client {
		// Atributos
		public Server Server { get; private set; }
		public Channel Channel { get; private set; }

		public Socket Socket { get; private set; }
		public byte [ ] Buffer { get; private set; }
		public bool Active { get; private set; }

		public ClientStatus Status { get; private set; }

		// Construtor
		public Client ( Server Server, Channel Channel, Socket s ) {
			this.Server = Server;
			this.Channel = Channel;

			this.Socket = s;
			this.Active = true;

			this.Status = ClientStatus.Connection;

			this.BeginReceive ( );
		}

		// Aguarda receber pacotes
		private void BeginReceive ( ) {
			try {
				if ( this.Active ) {
					this.Buffer = new byte [ 1024 ];
					this.Socket.BeginReceive ( this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback ( this.OnReceive ), null );
				}
			} catch ( Exception ex ) {
				Log.Error ( ex );
			}
		}
		private void OnReceive ( IAsyncResult ar ) {
			try {
				int size = 0;

				try { size = this.Socket.EndReceive ( ar ); } catch { size = 0; }

				if ( size <= 0 ) {
					this.Close ( );
				} else {
					byte [ ] tmp = this.Buffer.Take ( size ).ToArray ( );

					if ( this.Status == ClientStatus.Connection ) {
						if ( size == 4 ) {
							return;
						} else if ( size == 120 ) {
							tmp = tmp.Skip ( 4 ).ToArray ( );
						} else {
							this.Status = ClientStatus.Login;
						}
					}

					PSecurity.Decrypt ( tmp );

					Log.Normal ( $"Size: {tmp.Length:N0}{Environment.NewLine}{string.Join ( ", ", tmp.Select ( a => $"0x{$"{a:X}".PadLeft ( 2, '0' )}" ) )}" );
				}
			} catch ( Exception ex ) {
				Log.Error ( ex );
			} finally {
				this.BeginReceive ( );
			}
		}

		// Desconecta o cliente
		public void Close ( ) {
			if ( this.Active ) {
				this.Active = false;

				this.Socket.Close ( );
				this.Socket = null;

				this.Channel.Clients.Remove ( this );
			}
		}
	}
}