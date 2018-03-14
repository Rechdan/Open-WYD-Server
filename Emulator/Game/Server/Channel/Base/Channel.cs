using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Channel {
		// Atributos
		public Server Server { get; private set; }

		public Socket Socket { get; private set; }
		public bool Active { get; private set; }

		public List<Client> Clients { get; private set; }

		// Construtor
		public Channel ( Server Server , string ip ) {
			this.Server = Server;

			if ( IPAddress.TryParse ( ip , out IPAddress ipad ) ) {
				IPEndPoint ipep = new IPEndPoint ( ipad , 8281 );

				this.Socket = new Socket ( AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp );
				this.Socket.Bind ( ipep );
				this.Socket.Listen ( 0 );

				this.Active = true;

				this.Clients = new List<Client> ( );

				this.BeginAccept ( );

				Log.Information ( $"Canal {this.Socket.LocalEndPoint} iniciado!" );
			}
		}

		// Aceita conexões
		private void BeginAccept ( ) {
			try {
				this.Socket.BeginAccept ( new AsyncCallback ( this.OnAccept ) , null );
			} catch ( Exception ex ) {
				Log.Error ( ex );
			}
		}
		private void OnAccept ( IAsyncResult ar ) {
			try {
				Socket s = null;

				try { s = this.Socket.EndAccept ( ar ); } catch { s = null; }

				if ( s != null ) {
					this.Clients.Add ( new Client ( this.Server , this , s ) );
				}
			} catch ( Exception ex ) {
				Log.Error ( ex );
			} finally {
				this.BeginAccept ( );
			}
		}

		// Task
		public void OnTask ( ) {
			// Varre os clientes
			// .ToList serve para se algum cliente for removido não dar erro no .ForEach
			this.Clients.ToList ( ).ForEach ( c => c.OnTask ( ) );
		}

		// Retorna ID de cliente disponível
		public short GetClientId ( ) {
			for ( short i = Config.Values.Clients.MinCid ; i <= Config.Values.Clients.MaxCid ; i++ ) {
				if ( !this.Clients.Exists ( a => a.ClientId == i ) ) {
					return i;
				}
			}

			return -1;
		}
	}
}