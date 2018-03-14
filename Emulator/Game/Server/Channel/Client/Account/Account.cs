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
		public bool Login ( string UserName , string Password ) {
			if ( UserName == null ) {
				throw new Exception ( "UserName == null" );
			} else if ( Password == null ) {
				throw new Exception ( "Password == null" );
			}

			this.UserName = UserName;
			this.Password = Password;
			this.Numeric = $"{Config.Random.Next ( 1000 , 9999 )}";

			this.Characters = new Character [ 4 ];

			return true;
		}

		public bool CreateCharacter ( string Name , int ClassInfo , int Slot ) {
			if ( Name == null ) {
				throw new Exception ( "Name == null" );
			} else if ( ClassInfo < 0 || ClassInfo > 3 ) {
				throw new Exception ( "ClassInfo < 0 || ClassInfo > 3" );
			} else if ( Slot < 0 || Slot > 3 ) {
				throw new Exception ( "Slot < 0 || Slot > 3" );
			} else if ( this.Characters [ Slot ] != null ) {
				throw new Exception ( $"this.Characters [ {Slot} ] != null" );
			}

			SMob mob;

			switch ( ClassInfo ) {
				case 1: mob = SMob.FM ( Name ); break;
				case 2: mob = SMob.BM ( Name ); break;
				case 3: mob = SMob.HT ( Name ); break;
				default: mob = SMob.TK ( Name ); break;
			}

			this.Characters [ Slot ] = new Character ( mob );

			Functions.GetCurrentScore ( this.Characters [ Slot ] , true );

			return true;
		}

		public bool DeleteCharacter ( string Name , int Slot ) {
			if ( Name == null ) {
				throw new Exception ( "Name == null" );
			} else if ( Slot < 0 || Slot > 3 ) {
				throw new Exception ( "Slot < 0 || Slot > 3" );
			} else if ( this.Characters [ Slot ] == null ) {
				throw new Exception ( $"this.Characters [ {Slot} ] == null" );
			}

			this.Characters [ Slot ] = null;

			return true;
		}
	}
}