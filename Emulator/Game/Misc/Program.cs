using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator {
	class Program {
		static void Main ( string [ ] args ) {
			try {
				Config.Initialize ( );
			} catch ( Exception ex ) {
				Log.Error ( ex );
			} finally {
				Process.GetCurrentProcess ( ).WaitForExit ( );
			}
		}
	}
}