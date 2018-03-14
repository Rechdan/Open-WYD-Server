using System;
using System.Diagnostics;

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