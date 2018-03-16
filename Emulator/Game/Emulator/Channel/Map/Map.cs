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
	}
}