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

		public bool Respawn { get; set; }

		// Construtor
		public Surround ( object Obj ) {
			this.Obj = Obj;

			this.Surrounds = new List<object> ( );

			this.Respawn = true;
		}

		// Retorna os arredores
		public List<object> GetSurrounds ( ) {
			if ( this.Surrounds == null ) {
				throw new Exception ( "this.Surrounds == null" );
			}

			return this.Surrounds;
		}

		// Adiciona objeto aos arredores
		public void AddToSurrounds ( object o ) {
			if ( this.Surrounds == null ) {
				throw new Exception ( "this.Surrounds == null" );
			} else if ( o == null ) {
				throw new Exception ( "o == null" );
			}

			this.Surrounds.Add ( o );
		}

		// Remove objeto dos arredores
		public void RmvFromSurrounds ( object o ) {
			if ( this.Surrounds == null ) {
				throw new Exception ( "this.Surrounds == null" );
			} else if ( o == null ) {
				throw new Exception ( "o == null" );
			}

			this.Surrounds.Remove ( o );
		}

		// Atualiza os arredores
		public void UpdateSurrounds ( Action<List<object>> Same , Action<List<object>> Entered , Action<List<object>> Left ) {
			// Prepara lista pros novos arredores
			List<object> surrounds = new List<object> ( );

			// Gyarda mapa e posição
			Map map = null;
			Coord src = null;

			// Lê mapa e posição
			switch ( this.Obj ) {
				case Client client: {
					map = client.Map;
					src = map?.GetCoord ( client.Character.Mob.LastPosition );
					break;
				}
			}

			// Retorna lista com os arredores novos
			if ( map != null ) {
				for ( int x = src.X - Config.Values.Field.View ; x <= src.X + Config.Values.Field.View ; x++ ) {
					for ( int y = src.Y - Config.Values.Field.View ; y <= src.Y + Config.Values.Field.View ; y++ ) {
						Coord coord = map.GetCoord ( x , y );

						if ( coord != null ) {
							if ( coord.Client != null ) {
								surrounds.Add ( coord.Client );
							}
						}
					}
				}
			}

			// Remove valores inválidos
			surrounds.RemoveAll ( a => a == null );
			surrounds.Remove ( this.Obj );

			// Retorna listas com objetos novos, iguais e que saíram
			List<object>
				same = this.Surrounds.Intersect ( surrounds ).ToList ( ),
				entered = surrounds.Except ( this.Surrounds ).ToList ( ),
				left = this.Surrounds.Except ( surrounds ).ToList ( );

			// Adiciona este a lista dos que entraram
			entered.ForEach ( a => {
				switch ( a ) {
					case Client client: client.Surround.AddToSurrounds ( this.Obj ); break;
				}
			} );

			// Remove este da lista dos que saíram
			left.ForEach ( a => {
				switch ( a ) {
					case Client client: client.Surround.RmvFromSurrounds ( this.Obj ); break;
				}
			} );

			// Chama as ações
			Same?.Invoke ( same );
			Entered?.Invoke ( entered );
			Left?.Invoke ( left );

			// Atualiza a lista dos arredores
			this.Surrounds.Clear ( );
			this.Surrounds.AddRange ( surrounds );
		}
	}
}