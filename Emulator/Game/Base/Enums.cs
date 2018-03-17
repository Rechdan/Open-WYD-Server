namespace Emulator {
	// Tipos de visão
	public enum EnterVision : byte {
		Normal = 1,
		LogIn = 2,
		Teleport = 3,
		Summon = 4,

		Flexao = 16,
		Sentado = 32
	}

	public enum LeaveVision : int {
		Normal = 0,
		Morrer = 1,
		LogOut = 2,
		Teleport = 3
	}
}