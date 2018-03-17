using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Map {
		// Atributos
		public Channel Channel { get; private set; }

		private Coord [ , ] Coords { get; set; }

		// Construtor
		public Map ( Channel Channel ) {
			this.Channel = Channel;

			this.Coords = new Coord [ 4096 , 4096 ];
		}

		// Retorna a Coord
		public Coord GetCoord ( SPosition pos ) => GetCoord ( pos.X , pos.Y );
		public Coord GetCoord ( int X , int Y ) {
			if ( X < 0 || X > 4095 ) {
				throw new Exception ( $"X: 0 > {X} > 4095" );
			} else if ( Y < 0 || Y > 4095 ) {
				throw new Exception ( $"Y: 0 > {Y} > 4095" );
			}

			ref Coord coord = ref this.Coords [ X , Y ];

			if ( coord == null ) {
				coord = new Coord ( X , Y );
			}

			return coord;
		}

		// Task
		public void OnTask ( ) {
		}
	}
}