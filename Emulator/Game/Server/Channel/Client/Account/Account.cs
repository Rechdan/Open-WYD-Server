using System;

namespace Emulator {
	public class Account {
		// Atributos
		public string UserName { get; private set; }
		public string Password { get; private set; }
		public string Numeric { get; private set; }

		public Character [ ] Characters { get; private set; }

		// Construtor
		public Account ( ) {
			this.UserName = "";
			this.Password = "";
			this.Numeric = "";

			this.Characters = null;
		}

		// Métodos
		public bool SetLogin ( string UserName , string Password , string Numeric , Character [ ] Characters ) {
			this.UserName = UserName;
			this.Password = Password;
			this.Numeric = Numeric;

			this.Characters = Characters;

			return true;
		}
	}
}