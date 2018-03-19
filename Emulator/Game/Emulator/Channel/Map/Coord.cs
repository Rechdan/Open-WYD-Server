namespace Emulator {
	public class Coord {
		// Atributos
		public int X { get; private set; }
		public int Y { get; private set; }

		public Client Client { get; set; }

		public bool CanWalk => this.Client == null;

		public SPosition Position => SPosition.New ( this );

		// Construtor
		public Coord ( int X , int Y ) {
			this.X = X;
			this.Y = Y;
		}
	}
}