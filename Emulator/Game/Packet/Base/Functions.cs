﻿using System;
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
			}
			else {
				for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
					if ( Item.Ef [ i ].Type == 43 ) {
						value = Item.Ef [ i ].Value;
						break;
					}
					else if ( Item.Ef [ i ].Type >= 116 && Item.Ef [ i ].Type <= 125 ) {
						value = Item.Ef [ i ].Value;
						colored = true;
						break;
					}
				}

				if ( value > 9 && !colored ) {
					if ( value >= 230 && value <= 233 ) {
						value = 10;
					}
					else if ( value >= 234 && value <= 237 ) {
						value = 11;
					}
					else if ( value >= 238 && value <= 241 ) {
						value = 12;
					}
					else if ( value >= 242 && value <= 245 ) {
						value = 13;
					}
					else if ( value >= 246 && value <= 249 ) {
						value = 14;
					}
					else if ( value >= 250 && value <= 253 ) {
						value = 15;
					}
					else {
						value = 0;
					}
				}
				else if ( colored ) {
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
			}
			else {
				for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
					if ( Item.Ef [ i ].Type == 43 ) {
						value = Item.Ef [ i ].Value;
						break;
					}
					else if ( Item.Ef [ i ].Type >= 116 && Item.Ef [ i ].Type <= 125 ) {
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

		// Retorna o valor da refinação
		public static byte GetItemSanc ( SItem Item ) {
			for ( int i = 0 ; i < Item.Ef.Length ; i++ ) {
				if ( Item.Ef [ i ].Type == 43 || Item.Ef [ i ].Type >= 116 && Item.Ef [ i ].Type <= 125 ) {
					return Item.Ef [ i ].Value;
				}
			}

			return 0;
		}

		// Retorna quantas vezes o valor da refinação aumenta o status do item
		public static double GetItemSancValue ( int Slot , byte Sanc ) {
			if ( Slot == 0 || Slot == 14 ) {
				return 0;
			}
			else {
				if ( Slot >= 1 && Slot <= 7 ) {
					switch ( Sanc ) {
						case 1:
							return 1.1;
						case 2:
							return 1.2;
						case 3:
							return 1.3;
						case 4:
							return 1.4;
						case 5:
							return 1.5;
						case 6:
							return 1.6;
						case 7:
							return 1.7;
						case 8:
							return 1.8;
						case 9:
							return 1.9;
						case 230:
						case 231:
						case 232:
						case 233:
							return 2;
						case 234:
						case 235:
						case 236:
						case 237:
							return 2.2;
						case 238:
						case 239:
						case 240:
						case 241:
							return 2.5;
						case 242:
						case 243:
						case 244:
						case 245:
							return 2.8;
						case 246:
						case 247:
						case 248:
						case 249:
							return 3.2;
						case 250:
						case 251:
						case 252:
						case 253:
							return 3.7;
						default:
							return 1;
					}
				}
				else if ( Slot >= 8 && Slot <= 15 ) {
					switch ( Sanc ) {
						case 1:
							return 1.1;
						case 2:
							return 1.2;
						case 3:
							return 1.3;
						case 4:
							return 1.4;
						case 5:
							return 1.5;
						case 6:
							return 1.6;
						case 7:
							return 1.7;
						case 8:
							return 1.8;
						case 9:
							return 2;
						case 230:
						case 231:
						case 232:
						case 233:
							return 2.2;
						case 234:
						case 235:
						case 236:
						case 237:
							return 2.5;
						case 238:
						case 239:
						case 240:
						case 241:
							return 2.8;
						case 242:
						case 243:
						case 244:
						case 245:
							return 3.2;
						case 246:
						case 247:
						case 248:
						case 249:
							return 3.7;
						case 250:
						case 251:
						case 252:
						case 253:
							return 4;
						default:
							return 1;
					}
				}
				else {
					return 1;
				}
			}
		}

		// Atualiza os status do personagem
		public static void GetCurrentScore ( Character Character , bool UP ) {
			ref SMob mob = ref Character.Mob;

			Score status = Score.New ( );

			int
				level = mob.BaseStatus.Level,
				statusPoint = 0,
				masterPoint = 0,
				skillPoint = 0;

			double
				moveSpeed = 2,
				bootsSpeed = 0;

			#region Mounts
			double
				mountDamage = 0,
				mountMagic = 0,
				mountEvasion = 0,
				mountResist = 0,
				mountSpeed = 0;

			byte
				MountLevel = mob.Equip [ 14 ].Ef [ 1 ].Type;

			short montID = mob.Equip [ 14 ].Id;

			if ( montID >= 2360 && montID <= 2361 ) {
				mountSpeed = 4;
			}
			else if ( montID >= 2362 && montID <= 2365 ) {
				mountSpeed = 5;
			}
			else if ( montID >= 2366 && montID <= 2389 ) {
				mountSpeed = 6;
			}

			switch ( montID ) {
				#region Porco
				case 2360:
					break;
				#endregion
				#region Javali
				case 2361:
					break;
				#endregion
				#region Lobo
				case 2362:
					mountDamage = 10 + ( MountLevel * 0.5 );
					mountMagic = 1 + ( MountLevel * 0.1 );
					break;
				#endregion
				#region Dragão Menor
				case 2363:
					mountDamage = 16 + ( MountLevel * 0.8 );
					mountMagic = 2 + ( MountLevel * 0.15 );
					break;
				#endregion
				#region Urso
				case 2364:
					mountDamage = 20 + ( MountLevel * 1.0 );
					mountMagic = 3 + ( MountLevel * 0.2 );
					break;
				#endregion
				#region Dente de Sabre
				case 2365:
					mountDamage = 30 + ( MountLevel * 1.5 );
					mountMagic = 3 + ( MountLevel * 0.25 );
					break;
				#endregion
				#region Cavalo sem Sela N
				case 2366:
					mountDamage = 50 + ( MountLevel * 2.5 );
					mountMagic = 7 + ( MountLevel * 0.5 );
					mountEvasion = 4;
					break;
				#endregion
				#region Cavalo Fantasm N
				case 2367:
					mountDamage = 60 + ( MountLevel * 3.0 );
					mountMagic = 9 + ( MountLevel * 0.6 );
					mountEvasion = 5;
					break;
				#endregion
				#region Cavalo Leve N
				case 2368:
					mountDamage = 70 + ( MountLevel * 3.5 );
					mountMagic = 9 + ( MountLevel * 0.65 );
					mountEvasion = 6;
					break;
				#endregion
				#region Cavalo Equip N
				case 2369:
					mountDamage = 80 + ( MountLevel * 4.0 );
					mountMagic = 10 + ( MountLevel * 0.7 );
					mountEvasion = 7;
					break;
				#endregion
				#region Andaluz N
				case 2370:
					mountDamage = 100 + ( MountLevel * 5.0 );
					mountMagic = 12 + ( MountLevel * 0.85 );
					mountEvasion = 8;
					break;
				#endregion
				#region Cavalo sem Sela B
				case 2371:
					mountDamage = 50 + ( MountLevel * 2.5 );
					mountMagic = 7 + ( MountLevel * 0.5 );
					mountResist = 16;
					break;
				#endregion
				#region Cavalo Fantasm B
				case 2372:
					mountDamage = 60 + ( MountLevel * 3.0 );
					mountMagic = 9 + ( MountLevel * 0.6 );
					mountResist = 20;
					break;
				#endregion
				#region Cavalo Leve B
				case 2373:
					mountDamage = 70 + ( MountLevel * 3.5 );
					mountMagic = 9 + ( MountLevel * 0.65 );
					mountResist = 24;
					break;
				#endregion
				#region Cavalo Equip B
				case 2374:
					mountDamage = 80 + ( MountLevel * 4.0 );
					mountMagic = 10 + ( MountLevel * 0.7 );
					mountResist = 28;
					break;
				#endregion
				#region Andaluz B
				case 2375:
					mountDamage = 100 + ( MountLevel * 5.0 );
					mountMagic = 12 + ( MountLevel * 0.85 );
					mountResist = 32;
					break;
				#endregion
				#region Fenrir
				case 2376:
					mountDamage = 110 + ( MountLevel * 5.5 );
					mountMagic = 13 + ( MountLevel * 0.9 );
					break;
				#endregion
				#region Dragão
				case 2377:
					mountDamage = 120 + ( MountLevel * 6 );
					mountMagic = 13 + ( MountLevel * 0.9 );
					break;
				#endregion
				#region Fenrir das Sombrar
				case 2378:
					mountDamage = 130 + ( MountLevel * 6.5 );
					mountMagic = 15 + ( MountLevel * 1.0 );
					mountEvasion = 6;
					mountResist = 28;
					break;
				#endregion
				#region Tigre de Fogo
				case 2379:
					mountDamage = 130 + ( MountLevel * 6.5 );
					mountMagic = 15 + ( MountLevel * 0.9 );
					mountEvasion = 6;
					mountResist = 28;
					break;
				#endregion
				#region Dragão Vermelho
				case 2380:
					mountDamage = 140 + ( MountLevel * 7 );
					mountMagic = 16 + ( MountLevel * 1.1 );
					mountEvasion = 8;
					mountResist = 32;
					break;
				#endregion
				#region Unicórnio
				case 2381:
					mountDamage = 114 + ( MountLevel * 5.7 );
					mountMagic = 13 + ( MountLevel * 0.9 );
					mountEvasion = 2;
					mountResist = 16;
					break;
				#endregion
				#region Pegasus
				case 2382:
					mountDamage = 114 + ( MountLevel * 5.7 );
					mountMagic = 13 + ( MountLevel * 0.9 );
					mountEvasion = 3;
					mountResist = 8;
					break;
				#endregion
				#region Unisus
				case 2383:
					mountDamage = 114 + ( MountLevel * 5.7 );
					mountMagic = 13 + ( MountLevel * 0.9 );
					mountEvasion = 4;
					mountResist = 12;
					break;
				#endregion
				#region Grifo
				case 2384:
					mountDamage = 118 + ( MountLevel * 5.9 );
					mountMagic = 14 + ( MountLevel * 0.95 );
					mountEvasion = 3;
					mountResist = 20;
					break;
				#endregion
				#region Hipogrifo
				case 2385:
					mountDamage = 120 + ( MountLevel * 6 );
					mountMagic = 14 + ( MountLevel * 0.95 );
					mountEvasion = 4;
					mountResist = 16;
					break;
				#endregion
				#region Grifo Sangrento
				case 2386:
					mountDamage = 120 + ( MountLevel * 6 );
					mountMagic = 14 + ( MountLevel * 0.95 );
					mountEvasion = 5;
					mountResist = 16;
					break;
				#endregion
				#region Svadilfari
				case 2387:
					mountDamage = 120 + ( MountLevel * 6 );
					mountMagic = 6 + ( MountLevel * 0.4 );
					mountEvasion = 6;
					mountResist = 28;
					break;
				#endregion
				#region Sleipnir
				case 2388:
					mountDamage = 60 + ( MountLevel * 3 );
					mountMagic = 14 + ( MountLevel * 0.95 );
					mountEvasion = 6;
					mountResist = 28;
					break;
				#endregion
				#region Helriohdon
				case 2389:
					mountDamage = 120 + ( MountLevel * 6 );
					mountMagic = 15 + ( MountLevel * 1 );
					mountEvasion = 8;
					mountResist = 28;
					break;
					#endregion
			}

			status.Attack += mountDamage;
			status.Magic += mountMagic;

			status.Evasion += mountEvasion;

			status.resistFire += mountResist;
			status.resistIce += mountResist;
			status.resistHoly += mountResist;
			status.resistThunder += mountResist;
			#endregion

			#region Equips
			for ( int i = 0 ; i < mob.Equip.Length ; i++ ) {
				if ( i == 6 || i == 7 ) {
					continue;
				}

				short itemId = mob.Equip [ i ].Id;

				if ( itemId > 0 ) {
					SItemList itemList = Config.Itemlist [ itemId ];

					byte sanc = GetItemSanc ( mob.Equip [ i ] );
					double sanc_value = GetItemSancValue ( i , sanc );

					status += ( ( itemList + mob.Equip [ i ].Ef ) * sanc_value );

					if ( i == 2 || i == 3 ) {
						if ( sanc == 9 || ( sanc >= 230 && sanc <= 253 ) ) {
							status.Defense += 25;
						}
					}
					else if ( i == 5 ) {
						if ( sanc == 9 ) {
							bootsSpeed += 1;
						}
						else if ( sanc >= 230 && sanc <= 253 ) {
							bootsSpeed += 2;
						}
					}
				}
			}

			bootsSpeed += status.MoveSpeed;

			moveSpeed += ( bootsSpeed > 3 ? 3 : bootsSpeed );

			//status.Add ( mob.GetWeaponStatus ( ) );
			#endregion

			#region Pontos
			switch ( mob.Equip [ 0 ].Ef [ 1 ].Value ) {
				case 1: { // Mortal
					for ( int i = 0 ; i < level ; i++ ) {
						if ( i < 254 ) {
							statusPoint += 5;
						}
						else if ( i < 299 ) {
							statusPoint += 10;
						}
						else if ( i < 354 ) {
							statusPoint += 20;
						}
						else if ( i < 399 ) {
							statusPoint += 12;
						}

						if ( i < 199 ) {
							skillPoint += 3;
						}
						else if ( i < 354 ) {
							skillPoint += 4;
						}
						else if ( i < 399 ) {
							skillPoint += 3;
						}

						masterPoint += 2;
					}

					break;
				}

				case 2: { // Arch
					for ( int i = 0 ; i < level ; i++ ) {
						if ( i < 354 ) {
							statusPoint += 6;
						}
						else if ( i < 399 ) {
							statusPoint += 12;
						}

						if ( i < 354 ) {
							skillPoint += 4;
						}
						else if ( i < 399 ) {
							skillPoint += 2;
						}

						if ( i < 199 ) {
							masterPoint += 2;
						}
						else if ( i < 354 ) {
							masterPoint += 3;
						}
						else if ( i < 399 ) {
							masterPoint += 1;
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

			statusPoint -= mob.BaseStatus.Str + mob.BaseStatus.Int + mob.BaseStatus.Dex + mob.BaseStatus.Con;
			masterPoint -= mob.BaseStatus.Master.Sum ( a => a );

			/*bool[] skills = character.skills;

			for (int i = 0, index = mob.ClassInfo * 24; i < 24; i++)
			{
				if (skills[i])
				{
					skillP -= CG.skilldata[index + i].points;
				}
			}*/

			mob.StatusPoint = ( short ) ( statusPoint );
			mob.MasterPoint = ( short ) ( masterPoint );
			mob.SkillPoint = ( short ) ( skillPoint );
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
			}
			else if ( mob.ClassInfo == 1 ) { // FM
				status.Str += 5f;
				status.Int += 8f;
				status.Dex += 5f;
				status.Con += 5f;

				status.HP += 60f + ( ( level + status.Con ) * 1f );
				status.MP += 65f + ( ( level + status.Int ) * 3f );
			}
			else if ( mob.ClassInfo == 2 ) { // BM
				status.Str += 6f;
				status.Int += 6f;
				status.Dex += 9f;
				status.Con += 5f;

				status.HP += 70f + ( ( level + status.Con ) * 1f );
				status.MP += 55f + ( ( level + status.Int ) * 2f );
			}
			else if ( mob.ClassInfo == 3 ) { // HT
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

			moveSpeed = ( moveSpeed > 6 ? 6 : ( moveSpeed < 1 ? 1 : moveSpeed ) );
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

			mob.GameStatus.MobSpeed = ( byte ) ( mountSpeed > 0 ? mountSpeed : moveSpeed );
			#endregion

			#region Level UP
			if ( UP ) {
				mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
				mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
			}
			else {
				if ( mob.GameStatus.CurHP > mob.GameStatus.MaxHP ) {
					mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
				}

				if ( mob.GameStatus.CurMP > mob.GameStatus.MaxMP ) {
					mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
				}
			}
			#endregion
		}

		// Retorna uma coordenada livre para respawn
		public static Coord GetFreeRespawnCoord ( Map Map , Character Character ) {
			int city = Character.Mob.CityId;

			int minX, maxX, minY, maxY;

			// Filtra a cidade
			switch ( city ) {
				case 0: minX = 2085; maxX = 2110; minY = 2095; maxY = 2108; break; // 364 espaços para logar
				default: minX = 0; maxX = 0; minY = 0; maxY = 0; break;
			}

			// Válida as coordenadas
			if ( minX == 0 || maxX == 0 || minY == 0 || maxY == 0 ) {
				throw new Exception ( $"CityId {city} not found" );
			}

			// Prepara uma lista de coordenadas
			List<Coord> coords = new List<Coord> ( );

			for ( int x = minX ; x <= maxX ; x++ ) {
				for ( int y = minY ; y <= maxY ; y++ ) {
					Coord coord = Map.GetCoord ( x , y );

					if ( coord.CanWalk ) {
						coords.Add ( coord );
					}
				}
			}

			// Remove as coordenadas que não se pode andar
			coords.RemoveAll ( a => !a.CanWalk );

			// Verifica se há coordenadas disponíveis
			if ( coords.Count > 0 ) {
				// Retorna uma coordenada aleatória
				return coords [ Config.Random.Next ( 0 , coords.Count ) ];
			}

			return null;
		}

		// Remove cliente do mundo
		public static void RemoveFromWorld ( Client client ) {
			// Limpa cliente da sua posição
			client.Map.GetCoord ( client.Character.Mob.LastPosition ).Client = null;

			// Limpa a variável do mapa para que, no UpdateSurrounds, todos os clientes que tenham a visão deste o removam
			client.Map = null;

			// Prepara pacote de logout
			P_165 p165 = P_165.New ( client.ClientId , LeaveVision.LogOut );

			// Atualiza os arredores, agora sem esse cliente
			client.Surround.UpdateSurrounds ( null , null , left => {
				// Varre os que estavam na visão
				left.ForEach ( a => {
					switch ( a ) {
						case Client client2: {
							// Envia o logout para os outros clientes
							client2.Send ( p165 );
							break;
						}
					}
				} );
			} );
		}
	}
}