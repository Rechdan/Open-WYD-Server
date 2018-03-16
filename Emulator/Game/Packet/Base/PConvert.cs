using System;
using System.Runtime.InteropServices;

namespace Emulator {
	public static class PConvert {
		public static unsafe T ToStruct<T> ( byte [ ] data ) => ToStruct<T> ( data , 0 );
		public static unsafe T ToStruct<T> ( byte [ ] data , int start ) {
			fixed ( byte* pBuffer = data ) {
				return ( T ) ( Marshal.PtrToStructure ( new IntPtr ( ( void* ) ( &pBuffer [ start ] ) ) , typeof ( T ) ) );
			}
		}

		public static unsafe byte [ ] ToByteArray<T> ( T str ) {
			byte [ ] data = new byte [ Marshal.SizeOf ( str ) ];

			fixed ( byte* buffer = data ) {
				Marshal.StructureToPtr ( str , new IntPtr ( ( void* ) buffer ) , true );
			}

			return data;
		}
	}
}