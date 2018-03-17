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

		public ClientStatus Status { get; set; }

		public DateTime ConnectionTime { get; private set; }

		// Conta
		public Account Account { get; private set; }

		public int ClientId { get; set; }

		public Map Map { get; set; }

		public Character Character { get; set; }

		public Surround Surround { get; set; }

		// Construtor
		public Client ( Server Server , Channel Channel , Socket Socket ) {
			this.Server = Server;
			this.Channel = Channel;

			this.Socket = Socket;
			this.Active = true;

			this.Status = ClientStatus.Connection;

			this.ConnectionTime = Config.Time;

			this.Account = new Account ( );

			Log.Conn ( this , true );

			this.BeginReceive ( );
		}

		// Aguarda receber pacotes
		private void BeginReceive ( ) {
			try {
				if ( this.Active ) {
					this.Buffer = new byte [ 1024 ];
					this.Socket.BeginReceive ( this.Buffer , 0 , this.Buffer.Length , SocketFlags.None , new AsyncCallback ( this.OnReceive ) , null );
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
						}

						this.Status = ClientStatus.Login;
					}

					if ( size < 12 ) {
						this.Close ( );
						return;
					}

					PSecurity.Decrypt ( tmp );

					//Log.Normal ( $"Size: {tmp.Length:N0}{Environment.NewLine}{string.Join ( ", ", tmp.Select ( a => $"0x{$"{a:X}".PadLeft ( 2, '0' )}" ) )}" );

					PControl.Controller ( this , tmp );
				}
			} catch ( Exception ex ) {
				Log.Error ( ex );
			} finally {
				this.BeginReceive ( );
			}
		}

		// Envia pacote
		public void Send<T> ( T o ) {
			if ( o == null ) {
				throw new Exception ( "" );
			} else {
				byte [ ] send = PConvert.ToByteArray ( o );

				Log.Snd ( this , PConvert.ToStruct<SHeader> ( send ) );

				PSecurity.Encrypt ( send );

				this.Socket.BeginSend ( send , 0 , send.Length , SocketFlags.None , null , null );
			}
		}

		// Desconecta o cliente
		public void Close ( ) {
			if ( this.Active ) {
				// Define que o cliente está inativo
				this.Active = false;

				// Atualiza o status do cliente
				this.Status = ClientStatus.Disconnect;

				// Log
				Log.Conn ( this , false );

				// Fecha a conexão com o emulador
				this.Socket.Close ( 2000 );
				this.Socket = null;

				// Verifica se está no mundo
				if ( this.Status == ClientStatus.Game ) {
					Functions.RemoveFromWorld ( this );
				}

				// Remove o cliente do canal
				this.Channel.Clients.Remove ( this );
			}
		}
		public void Close ( string Reason ) {
			if ( Reason == null ) {
				throw new Exception ( "Reason == null" );
			}

			this.Send ( P_101.New ( Reason ) );
		}

		// Task
		public void OnTask ( ) {
			// Valida conexão
			if ( this.Status == ClientStatus.Connection ) {
				// Se já passou 3 segundos, corta conexão
				if ( ( Config.Time - this.ConnectionTime ).TotalSeconds >= 3d ) {
					this.Close ( );
					return;
				}
			}
		}
	}
}