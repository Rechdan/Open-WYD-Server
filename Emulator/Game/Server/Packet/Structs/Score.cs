using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		// Métodos
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

			return Current;
		}
	}
}