using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura da posição - size 4
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SPosition {
		// Atributos
		public short X; // 0 a 1	= 2
		public short Y; // 2 a 3	= 2

		// Construtores
		public static SPosition New ( ) => New ( 0 , 0 );
		public static SPosition New ( SPosition p ) => New ( p.X , p.Y );
		public static SPosition New ( Coord c ) => New ( c.X , c.Y );
		public static SPosition New ( int x , int y ) => new SPosition ( ) { X = ( short ) ( x ) , Y = ( short ) ( y ) };

		// Métodos
		public int GetDistance ( SPosition other ) {
			int x = Math.Abs ( this.X - other.X );
			int y = Math.Abs ( this.Y - other.Y );

			return ( x > y ? x : y );
		}

		// Altera as checagens de == e !=
		public static bool operator == ( SPosition a , SPosition b ) => ( a.X == b.X && a.Y == b.Y );
		public static bool operator != ( SPosition a , SPosition b ) => ( a.X != b.X || a.Y != b.Y );

		// Altera o ToString
		public override string ToString ( ) => $"{this.X},{this.Y}";

		// Obrigações do C# para quando sobrescrever == e !=
		public override int GetHashCode ( ) => base.GetHashCode ( );

		public override bool Equals ( object obj ) {
			switch ( obj ) {
				case SPosition position: return ( this.X == position.X && this.Y == position.Y );
				default: return false;
			}
		}
	}
}