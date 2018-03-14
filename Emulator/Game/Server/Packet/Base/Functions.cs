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
	}
}