using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Game {
		// Atributos
		public bool Active { get; private set; }
		public List<Server> Servers { get; private set; }

		// Construtor
		public Game ( ) {
			this.Active = false;
			this.Servers = new List<Server> ( );

			Log.Information ( "Game iniciado!" );
		}

		public Game AddServer ( Server s, Action<Server> a ) {
			this.Servers.Add ( s );
			a?.Invoke ( s );
			return this;
		}

		public void Run ( ) {
			Log.Line ( );

			if ( !this.Active ) {
				this.Active = true;

				Task.WaitAll ( this.Controller ( ) );

				this.Active = false;
			}
		}

		// Controlador de tudo do emulador
		private async Task Controller ( ) {
			while ( this.Active ) {
				try {
					// Varre os servidores
					this.Servers.ForEach ( s => s.OnTask ( ) );
				} catch ( Exception ex ) {
					Log.Error ( ex );
				} finally {
					await Task.Delay ( 1 );
				}
			}
		}
	}
}