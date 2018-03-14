using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Emulator {
	/// <summary>
	/// Criar personagem - size 36
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_20F {
		// Atributos
		public SHeader Header;      // 0 a 11		= 12

		public int Slot;            // 12 a 15	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] NameBytes;  // 16 a 27	= 12

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] Unk1;       // 28 a 31	= 4

		public int ClassInfo;       // 32 a 35	= 4

		// Ajudantes
		public string Name {
			get => Functions.GetString ( this.NameBytes );
			set {
				this.NameBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.NameBytes , 12 );
			}
		}

		// Controlador
		public static void Controller ( Client client , P_20F rcv ) {
			if ( !Regex.IsMatch ( rcv.Name , @"^[A-Za-z0-9-]{4,12}$" ) ) {
				client.Send ( P_101.New ( "Somente letras e números no nome. 4 a 12 caracteres." ) );
			} else {
				if ( !client.Account.CreateCharacter ( rcv.Name , rcv.ClassInfo , rcv.Slot ) ) {
					client.Send ( P_101.New ( "Algo não ocorreu corretamente. Tente novamente!" ) );
				} else {
					client.Send ( P_110.New ( client ) );
					client.Send ( P_101.New ( $"Personagem {rcv.Name} criado! Bom jogo!" ) );
				}
			}
		}
	}
}