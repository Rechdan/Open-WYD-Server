using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	/// <summary>
	/// Andar - size 52
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_36C {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		public SPosition Src;       // 12 a 15	= 4

		public int Type;            // 16 a 19	= 4
		public int Speed;           // 20 a 23	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] Directions;  // 24 a 35	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] Unk1;       // 36 a 47	= 12

		public SPosition Dst;       // 48 a 51	= 4

		// Construtores
		public static P_36C New ( int ClientId , SPosition Src , SPosition Dst , int Type ) => New ( ClientId , Src , Dst , Type , 0 , null );
		public static P_36C New ( int ClientId , SPosition Src , SPosition Dst , int Type , int Speed , byte [ ] Directions ) {
			P_36C tmp = new P_36C {
				Header = SHeader.New ( 0x36C , Marshal.SizeOf<P_36C> ( ) , ClientId ) ,

				Src = Src ,

				Type = Type ,
				Speed = Speed ,

				Directions = new byte [ 12 ] ,

				Unk1 = new byte [ 12 ] ,

				Dst = Dst ,
			};

			if ( Directions != null ) {
				Array.Copy ( Directions , tmp.Directions , tmp.Directions.Length );
			}

			return tmp;
		}

		// Controlador
		public static void Controller ( Client Client , P_36C Rcv , bool Is36C ) {
			// Guarda a posição original do personagem
			Coord cur = Client.Map.GetCoord ( Client.Character.Mob.LastPosition );

			// Guarda a posição de destino
			Coord dst = Client.Map.GetCoord ( Rcv.Dst );

			// Verifica se as posições de origem e destino são a mesma
			if ( Client.Character.Mob.LastPosition == Rcv.Dst ) {
				return;
			}
			// Verifica se o caminho de destino está vaziu
			else if ( !dst.CanWalk ) {
				// Manda cliente de volta pra posição inicial
				Log.Error ( 0 );
				Sendback ( );
				return;
			}
			// Verifica se a posição de origem está na lista de posições do personagem
			else if ( !Client.Character.Positions.Contains ( Rcv.Src ) ) {
				// Manda cliente de volta pra posição inicial
				Log.Error ( 1 );
				Sendback ( );
				return;
			}

			// Valida o caminho pelo HeightMap
			SPosition position = Rcv.Src;
			List<SPosition> positions = new List<SPosition> ( ) { position };

			int cHeight = Config.HeightMap ( position ), oHeight = cHeight;

			// Verifica qual pacote foi enviado
			if ( Is36C ) {
				foreach ( byte direction in Rcv.Directions ) {
					switch ( direction ) {
						case 0: continue; // Chegou ao fim

						case 49: position.X--; position.Y--; break; // -X -Y
						case 50: position.Y--; break; // -Y
						case 51: position.X++; position.Y--; break; // +X -Y
						case 52: position.X--; break; // -X
						case 54: position.X++; break; // +X
						case 55: position.X--; position.Y++; break; // -X +Y
						case 56: position.Y++; break; // +Y
						case 57: position.X++; position.Y++; break; // +X +Y

						default: Client.Close ( $"Unk direction: {direction}" ); break; // Coordenada inválida
					}

					// Guarda a altura do mapa de destino
					oHeight = Config.HeightMap ( position );

					// Verifica se a mudança de altura é maior que 9
					if ( Math.Abs ( cHeight - oHeight ) > 9 ) {
						// Manda cliente de volta pra posição inicial
						Log.Error ( 2 );
						Sendback ( );
						return;
					}

					// Atualiza a altura do mapa atual
					cHeight = oHeight;

					// Adiciona a posição a lista
					positions.Add ( position );
				}
			}
			else {
				// Adiciona a posição a lista
				positions.Add ( Rcv.Dst );
				// INCOMPLETO, NADA FEITO AQUI AINDA!!!
			}

			// Atualiza a lista de posições do personagem
			Client.Character.Positions.Clear ( );
			Client.Character.Positions.AddRange ( positions );

			// Atualiza a posição do personagem
			Client.Character.Mob.LastPosition = Rcv.Dst;

			// Atualiza a posição no mapa
			cur.Client = null;
			dst.Client = Client;

			// Pacote do andar
			P_36C p36C = New ( Client.ClientId , Rcv.Src , Rcv.Dst , Rcv.Type , Rcv.Speed , Rcv.Directions );

			// Atualiza os arredores
			Client.Surround.UpdateSurrounds ( same => {
				// Varre a lista
				foreach ( object o in same ) {
					switch ( o ) {
						case Client client2: {
							client2.Send ( p36C );
							break;
						}
					}
				}
			} , entered => {
				// Visão do cliente
				P_364 p364 = P_364.New ( Client.Character , EnterVision.Normal );
				p364.Position = SPosition.New ( cur );

				// Varre a lista
				foreach ( object o in entered ) {
					switch ( o ) {
						case Client client2: {
							client2.Send ( p364 );
							client2.Send ( p36C );
							Client.Send ( P_364.New ( client2.Character , EnterVision.Normal ) );
							break;
						}
					}
				}
			} , left => {
				// Visão do cliente
				P_165 p165 = P_165.New ( Client.ClientId , LeaveVision.Normal );

				// Varre a lista
				foreach ( object o in left ) {
					switch ( o ) {
						case Client client2: {
							client2.Send ( p36C );
							client2.Send ( p165 );
							Client.Send ( P_165.New ( client2.ClientId , LeaveVision.Normal ) );
							break;
						}
					}
				}
			} );

			// Funções ajudantes
			void Sendback ( ) {
				Client.Send ( New ( Client.ClientId , SPosition.New ( cur ) , SPosition.New ( cur ) , 1 , 0 , null ) );
			}
		}
	}
}