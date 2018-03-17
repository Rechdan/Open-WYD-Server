using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Surround {
		// Atributos
		public object Obj { get; private set; }

		private List<object> Surrounds { get; set; }

		// Construtor
		public Surround ( object Obj ) {
			this.Obj = Obj;

			this.Surrounds = new List<object> ( );
		}

		// Atualiza os arredores
		public void SetSurrounds ( List<object> Surrounds ) {
			this.Surrounds.Clear ( );

			this.Surrounds.AddRange ( Surrounds );
		}

		// Retorna os arredores
		public List<object> GetSurrounds ( ) {
			if ( this.Surrounds == null ) {
				throw new Exception ( "this.Surrounds == null" );
			}

			return this.Surrounds;
		}

		// Atualiza os arredores
		public List<object> UpdateSurrounds ( ) {
			List<object> surrounds = new List<object> ( );

			Map map = null;
			Coord src = null;

			switch ( this.Obj ) {
				case Client client: map = client.Map; src = map.GetCoord ( client.Character.Mob.LastPosition ); break;
			}

			for ( int x = src.X - Config.Values.Field.View ; x <= src.X + Config.Values.Field.View ; x++ ) {
				for ( int y = src.Y - Config.Values.Field.View ; y <= src.Y + Config.Values.Field.View ; y++ ) {
					Coord coord = map.GetCoord ( x , y );

					if ( coord != null ) {
						if ( coord.Client != null ) {
							surrounds.Add ( coord.Client );

							coord.Client.Surround.AddToSurrounds ( this.Obj );
						}
					}
				}
			}

			surrounds.RemoveAll ( a => a == null );
			surrounds.Remove ( this.Obj );

			return surrounds;
		}

		// Adiciona um objeto aos seus arredores
		public void AddToSurrounds ( object o ) {
			if ( this.Surrounds == null ) {
				throw new Exception ( "this.Surrounds == null" );
			} else if ( o == null ) {
				throw new Exception ( "o == null" );
			}

			this.Surrounds.Add ( o );
		}
	}
}