using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public class Character {
		// Atributos
		public SMob Mob;

		// Construtores
		public Character ( SMob Mob ) {
			this.Mob = Mob;
		}
	}
}