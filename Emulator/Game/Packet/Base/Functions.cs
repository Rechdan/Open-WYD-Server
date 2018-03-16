using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	public static class Functions {
		// Retorna uma string sem caracteres nulos
		public static string GetString ( byte [ ] data ) {
			if ( data == null ) {
				throw new Exception ( "data == null" );
			}

			return Config.Encoding.GetString ( data ).TrimEnd ( '\0' );
		}

		// Retorna o ID do item pra visão
		public static short GetItemID ( SItem Item , bool Mont ) {
			bool colored = false;
			byte value = 0;

			if ( Mont ) {
				value = ( byte ) ( Math.Floor ( ( double ) ( Item.Ef [ 1 ].Type / 10 ) ) );
			} else {
				for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
					if ( Item.Ef [ i ].Type == 43 ) {
						value = Item.Ef [ i ].Value;
						break;
					} else if ( Item.Ef [ i ].Type >= 116 && Item.Ef [ i ].Type <= 125 ) {
						value = Item.Ef [ i ].Value;
						colored = true;
						break;
					}
				}

				if ( value > 9 && !colored ) {
					if ( value >= 230 && value <= 233 ) {
						value = 10;
					} else if ( value >= 234 && value <= 237 ) {
						value = 11;
					} else if ( value >= 238 && value <= 241 ) {
						value = 12;
					} else if ( value >= 242 && value <= 245 ) {
						value = 13;
					} else if ( value >= 246 && value <= 249 ) {
						value = 14;
					} else if ( value >= 250 && value <= 253 ) {
						value = 15;
					} else {
						value = 0;
					}
				} else if ( colored ) {
					if ( value > 9 ) {
						value = 9;
					}
				}
			}

			return ( short ) ( Item.Id + ( value * 4096 ) );
		}

		// Retorna o código anct do item ou sua tintura
		public static byte GetAnctCode ( SItem Item , bool Mont ) {
			byte value = 0;

			if ( Mont ) {
				return 0;
			} else {
				for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
					if ( Item.Ef [ i ].Type == 43 ) {
						value = Item.Ef [ i ].Value;
						break;
					} else if ( Item.Ef [ i ].Type >= 116 && Item.Ef [ i ].Type <= 125 ) {
						return Item.Ef [ i ].Type;
					}
				}
			}

			if ( value == 0 ) {
				return 0;
			}

			if ( value < 230 ) {
				return 43;
			}

			switch ( value % 4 ) {
				case 0: return 0x30;
				case 1: return 0x40;
				case 2: return 0x10;
				case 3: return 0x20;
				default: return value;
			}
		}

		// Atualiza os status do personagem
		public static void GetCurrentScore ( Character character , bool UP ) {
			ref SMob mob = ref character.Mob;

			Score status = Score.New ( );

			int
				level = mob.BaseStatus.Level,
				StatusPoint = 0,
				MasterPoint = 0,
				SkillPoint = 0;

			double
				MoveSpeed = 2;

			#region Pontos
			switch ( mob.Equip [ 0 ].Ef [ 1 ].Value ) {
				case 1: { // Mortal
					for ( int i = 0 ; i < level ; i++ ) {
						if ( i < 254 ) {
							StatusPoint += 5;
						} else if ( i < 299 ) {
							StatusPoint += 10;
						} else if ( i < 354 ) {
							StatusPoint += 20;
						} else if ( i < 399 ) {
							StatusPoint += 12;
						}

						if ( i < 199 ) {
							SkillPoint += 3;
						} else if ( i < 354 ) {
							SkillPoint += 4;
						} else if ( i < 399 ) {
							SkillPoint += 3;
						}

						MasterPoint += 2;
					}

					break;
				}

				case 2: { // Arch
					for ( int i = 0 ; i < level ; i++ ) {
						if ( i < 354 ) {
							StatusPoint += 6;
						} else if ( i < 399 ) {
							StatusPoint += 12;
						}

						if ( i < 354 ) {
							SkillPoint += 4;
						} else if ( i < 399 ) {
							SkillPoint += 2;
						}

						if ( i < 199 ) {
							MasterPoint += 2;
						} else if ( i < 354 ) {
							MasterPoint += 3;
						} else if ( i < 399 ) {
							MasterPoint += 1;
						}
					}

					break;
				}

				case 3: { // Celestial
					break;
				}

				case 4: { // Sub Celestial
					break;
				}
			}

			StatusPoint -= mob.BaseStatus.Str + mob.BaseStatus.Int + mob.BaseStatus.Dex + mob.BaseStatus.Con;
			MasterPoint -= mob.BaseStatus.Master.Sum ( a => a );

			/*bool[] skills = character.skills;

			for (int i = 0, index = mob.ClassInfo * 24; i < 24; i++)
			{
				if (skills[i])
				{
					skillP -= CG.skilldata[index + i].points;
				}
			}*/

			mob.StatusPoint = ( short ) ( StatusPoint );
			mob.MasterPoint = ( short ) ( MasterPoint );
			mob.SkillPoint = ( short ) ( SkillPoint );
			#endregion

			#region Base
			status.Str += mob.BaseStatus.Str;
			status.Int += mob.BaseStatus.Int;
			status.Dex += mob.BaseStatus.Dex;
			status.Con += mob.BaseStatus.Con;

			status.Defense += 4f + level;

			for ( int i = 0 ; i < mob.BaseStatus.Master.Length ; i++ ) {
				status.Master [ i ] = mob.BaseStatus.Master [ i ];
			}

			if ( mob.ClassInfo == 0 ) { // TK
				status.Str += 8f;
				status.Int += 4f;
				status.Dex += 7f;
				status.Con += 6f;

				status.HP += 80f + ( ( level + status.Con ) * 3f );
				status.MP += 45f + ( ( level + status.Int ) * 1f );
			} else if ( mob.ClassInfo == 1 ) { // FM
				status.Str += 5f;
				status.Int += 8f;
				status.Dex += 5f;
				status.Con += 5f;

				status.HP += 60f + ( ( level + status.Con ) * 1f );
				status.MP += 65f + ( ( level + status.Int ) * 3f );
			} else if ( mob.ClassInfo == 2 ) { // BM
				status.Str += 6f;
				status.Int += 6f;
				status.Dex += 9f;
				status.Con += 5f;

				status.HP += 70f + ( ( level + status.Con ) * 1f );
				status.MP += 55f + ( ( level + status.Int ) * 2f );
			} else if ( mob.ClassInfo == 3 ) { // HT
				status.Str += 8f;
				status.Int += 9f;
				status.Dex += 13f;
				status.Con += 6f;

				status.HP += 75f + ( ( level + status.Con ) * 2f );
				status.MP += 60f + ( ( level + status.Int ) * 1f );
			}
			#endregion

			#region Status para Atk
			status.Attack += ( status.Str / 4 ) + ( status.Dex / 6 ) + ( status.Master [ 0 ] * 1.65 );

			status.AttackSpeed += ( status.AttackSpeed * 10 ) + ( status.Dex / 5 );
			#endregion

			#region Porcentagens
			status.HP += status.HP * ( status.PHP / 100d );
			status.MP += status.MP * ( status.PMP / 100d );
			#endregion

			#region Limits
			status.Attack = ( status.Attack > 32000 ? 32000 : ( status.Attack < 0 ? 0 : status.Attack ) );
			status.Defense = ( status.Defense > 32000 ? 32000 : ( status.Defense < 0 ? 0 : status.Defense ) );

			status.HP = ( status.HP > 32000 ? 32000 : ( status.HP < 0 ? 0 : status.HP ) );
			status.MP = ( status.MP > 32000 ? 32000 : ( status.MP < 0 ? 0 : status.MP ) );

			status.Str = ( status.Str > 32000 ? 32000 : ( status.Str < 0 ? 0 : status.Str ) );
			status.Int = ( status.Int > 32000 ? 32000 : ( status.Int < 0 ? 0 : status.Int ) );
			status.Dex = ( status.Dex > 32000 ? 32000 : ( status.Dex < 0 ? 0 : status.Dex ) );
			status.Con = ( status.Con > 32000 ? 32000 : ( status.Con < 0 ? 0 : status.Con ) );

			for ( int i = 0 ; i < status.Master.Length ; i++ ) {
				status.Master [ i ] = ( status.Master [ i ] > 255 ? 255 : ( status.Master [ i ] < 0 ? 0 : status.Master [ i ] ) );
			}

			status.resistFire = ( status.resistFire > 250 ? 250 : ( status.resistFire < 0 ? 0 : status.resistFire ) );
			status.resistIce = ( status.resistIce > 250 ? 250 : ( status.resistIce < 0 ? 0 : status.resistIce ) );
			status.resistHoly = ( status.resistHoly > 250 ? 250 : ( status.resistHoly < 0 ? 0 : status.resistHoly ) );
			status.resistThunder = ( status.resistThunder > 250 ? 250 : ( status.resistThunder < 0 ? 0 : status.resistThunder ) );

			status.Critical /= 4;
			status.Critical = status.Critical > 255 ? 255 : status.Critical;

			status.Magic /= 4;
			status.Magic = status.Magic > 65000 ? 65000 : status.Magic;

			status.AttackSpeed = ( status.AttackSpeed > ushort.MaxValue ? ushort.MaxValue : ( status.AttackSpeed < 0 ? 0 : status.AttackSpeed ) );

			status.Evasion = ( status.Evasion > short.MaxValue ? short.MaxValue : ( status.Evasion < 0 ? 0 : status.Evasion ) );

			MoveSpeed = ( MoveSpeed > 6 ? 6 : ( MoveSpeed < 1 ? 1 : MoveSpeed ) );
			#endregion

			#region Update MOB
			mob.GameStatus.Level = mob.BaseStatus.Level;

			mob.GameStatus.Attack = ( short ) ( status.Attack );
			mob.GameStatus.Defense = ( short ) ( status.Defense );

			mob.GameStatus.MaxHP = ( ushort ) ( status.HP );
			mob.GameStatus.MaxMP = ( ushort ) ( status.MP );

			mob.GameStatus.Str = ( short ) ( status.Str );
			mob.GameStatus.Int = ( short ) ( status.Int );
			mob.GameStatus.Dex = ( short ) ( status.Dex );
			mob.GameStatus.Con = ( short ) ( status.Con );

			//mob.attackSpeed = (ushort)(status.AttackSpeed);

			//mob.evasion = (short)(status.Evasion);

			mob.MagicIncrement = ( short ) ( status.Magic );
			mob.Critical = ( byte ) ( status.Critical );

			for ( int i = 0 ; i < status.Master.Length ; i++ ) {
				mob.GameStatus.Master [ i ] = ( byte ) ( status.Master [ i ] );
			}

			mob.ResistFire = ( byte ) ( status.resistFire );
			mob.ResistIce = ( byte ) ( status.resistIce );
			mob.ResistHoly = ( byte ) ( status.resistHoly );
			mob.ResistThunder = ( byte ) ( status.resistThunder );

			mob.GameStatus.MobSpeed = ( byte ) ( MoveSpeed );
			#endregion

			#region Level UP
			if ( UP ) {
				mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
				mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
			} else {
				if ( mob.GameStatus.CurHP > mob.GameStatus.MaxHP ) {
					mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
				}

				if ( mob.GameStatus.CurMP > mob.GameStatus.MaxMP ) {
					mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
				}
			}
			#endregion
		}
	}
}