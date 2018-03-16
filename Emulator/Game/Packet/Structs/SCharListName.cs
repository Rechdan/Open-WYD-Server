using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Nome do personagem na CharList - size 16
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SCharListName {
		// Atributos
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] NameBytes;  // 0 a 15	= 16

		// Ajudantes
		public string Name {
			get => Functions.GetString ( this.NameBytes );
			set {
				this.NameBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.NameBytes , 16 );
			}
		}

		// Construtores
		public static SCharListName New ( string Name ) {
			SCharListName tmp = new SCharListName {
				Name = Name
			};

			return tmp;
		}
	}
}