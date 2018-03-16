using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Server {
		// Atributos
		public string Name { get; private set; }
		public List<Channel> Channels { get; private set; }

		// Construtor
		public Server ( string Name ) {
			this.Name = Name;
			this.Channels = new List<Channel> ( );

			Log.Information ( $"Servidor {this.Name} iniciado!" );
		}

		public Server AddChannel ( Channel c , Action<Channel> a ) {
			this.Channels.Add ( c );
			a?.Invoke ( c );
			return this;
		}

		// Task
		public void OnTask ( ) {
			// Varre os canais
			this.Channels.ForEach ( c => c.OnTask ( ) );
		}
	}
}