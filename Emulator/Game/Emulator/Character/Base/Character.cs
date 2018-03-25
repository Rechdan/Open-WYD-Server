using System.Collections.Generic;

namespace Emulator {
	public class Character {
		// Atributos
		public SMob Mob;

		public List<SPosition> Positions { get; private set; }

		// Construtor
		public Character ( ) {
			this.Mob = default;

			this.Positions = new List<SPosition> ( );
		}
	}
}