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
	}
}