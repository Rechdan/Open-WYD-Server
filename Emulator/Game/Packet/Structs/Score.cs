namespace Emulator {
	/// <summary>
	/// Estrutura para guardar score
	/// </summary>
	public struct Score {
		// Atributos
		public double Attack;
		public double Defense;

		public double HP;
		public double MP;
		public double PHP;
		public double PMP;

		public double Str;
		public double Int;
		public double Dex;
		public double Con;

		public double [ ] Master;

		public double resistFire;
		public double resistIce;
		public double resistHoly;
		public double resistThunder;

		public double Critical;
		public double Magic;

		public double SaveMana;
		public double HPRegen;
		public double MPRegen;

		public double AttackSpeed;

		public double Evasion;

		public double MoveSpeed;

		public bool [ ] Equip;

		public byte Grid;

		// Construtores
		public static Score New ( ) {
			Score tmp = new Score ( ) {
				Attack = 0d ,
				Defense = 0d ,

				HP = 0d ,
				MP = 0d ,
				PHP = 0d ,
				PMP = 0d ,

				Str = 0d ,
				Int = 0d ,
				Dex = 0d ,
				Con = 0d ,

				Master = new double [ ] { 0d , 0d , 0d , 0d } ,

				resistFire = 0d ,
				resistIce = 0d ,
				resistHoly = 0d ,
				resistThunder = 0d ,

				Critical = 0d ,
				Magic = 0d ,

				SaveMana = 0d ,
				HPRegen = 0d ,
				MPRegen = 0d ,

				AttackSpeed = 0d ,

				Evasion = 0d ,

				MoveSpeed = 0d ,

				Equip = new bool [ 16 ] ,

				Grid = 0
			};

			return tmp;
		}

		// Operadores
		public static Score operator + ( Score Current , Score Other ) {
			Current.Attack += Other.Attack;
			Current.Defense += Other.Defense;

			Current.HP += Other.HP;
			Current.MP += Other.MP;
			Current.PHP += Other.PHP;
			Current.PMP += Other.PMP;

			Current.Str += Other.Str;
			Current.Int += Other.Int;
			Current.Dex += Other.Dex;
			Current.Con += Other.Con;

			for ( int i = 0 ; i < Current.Master.Length ; i++ ) {
				Current.Master [ i ] += Other.Master [ i ];
			}

			Current.resistFire += Other.resistFire;
			Current.resistIce += Other.resistIce;
			Current.resistHoly += Other.resistHoly;
			Current.resistThunder += Other.resistThunder;

			Current.Critical += Other.Critical;
			Current.Magic += Other.Magic;

			Current.SaveMana += Other.SaveMana;
			Current.HPRegen += Other.HPRegen;
			Current.MPRegen += Other.MPRegen;

			Current.AttackSpeed += Other.AttackSpeed;

			Current.Evasion += Other.Evasion;

			if ( Current.MoveSpeed < Other.MoveSpeed ) {
				Current.MoveSpeed = Other.MoveSpeed;
			}

			return Current;
		}

		public static Score operator + ( Score Current , SItemList Item ) {
			for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
				short val = Item.Ef [ i ].Value;

				switch ( Item.Ef [ i ].Index ) {
					case 2:
					case 67:
					case 73:
						Current.Attack += val;
						break;

					case 3:
					case 53:
					case 72:
						Current.Defense += val;
						break;

					case 4:
						Current.HP += val;
						break;

					case 5:
						Current.MP += val;
						break;

					case 7:
						Current.Str += val;
						break;

					case 8:
						Current.Int += val;
						break;

					case 9:
						Current.Dex += val;
						break;

					case 10:
						Current.Con += val;
						break;

					case 11:
						Current.Master [ 0 ] += val;
						break;

					case 12:
						Current.Master [ 1 ] += val;
						break;

					case 13:
						Current.Master [ 2 ] += val;
						break;

					case 14:
						Current.Master [ 3 ] += val;
						break;

					case 26:
						Current.AttackSpeed += val;
						break;

					case 29:
						Current.MoveSpeed += val;
						break;

					case 33:
						Current.Grid = ( byte ) ( val );
						break;

					case 42:
					case 71:
						Current.Critical += val;
						break;

					case 45:
					case 69:
						Current.PHP += val;
						break;

					case 46:
					case 70:
						Current.PMP += val;
						break;

					case 47:
						Current.HPRegen += val;
						break;

					case 48:
						Current.MPRegen += val;
						break;

					case 49:
						Current.resistFire += val;
						break;

					case 50:
						Current.resistIce += val;
						break;

					case 51:
						Current.resistHoly += val;
						break;

					case 52:
						Current.resistThunder += val;
						break;

					case 54:
						Current.resistFire += val;
						Current.resistIce += val;
						Current.resistHoly += val;
						Current.resistThunder += val;
						break;

					case 60:
					case 68:
						Current.Magic += val;
						break;

					case 74:
						for ( int j = 1 ; j < Current.Master.Length ; j++ ) {
							Current.Master [ j ] += val;
						}
						break;
				}
			}

			return Current;
		}

		public static Score operator * ( Score Current , double Multiplier ) {
			Current.Attack *= Multiplier;
			Current.Defense *= Multiplier;

			Current.HP *= Multiplier;
			Current.MP *= Multiplier;
			Current.PHP *= Multiplier;
			Current.PMP *= Multiplier;

			Current.Str *= Multiplier;
			Current.Int *= Multiplier;
			Current.Dex *= Multiplier;
			Current.Con *= Multiplier;

			for ( int i = 0 ; i < Current.Master.Length ; i++ ) {
				Current.Master [ i ] *= Multiplier;
			}

			Current.resistFire *= Multiplier;
			Current.resistIce *= Multiplier;
			Current.resistHoly *= Multiplier;
			Current.resistThunder *= Multiplier;

			Current.Critical *= Multiplier;
			Current.Magic *= Multiplier;

			Current.SaveMana *= Multiplier;
			Current.HPRegen *= Multiplier;
			Current.MPRegen *= Multiplier;

			Current.AttackSpeed *= Multiplier;

			Current.Evasion *= Multiplier;

			return Current;
		}
	}
}