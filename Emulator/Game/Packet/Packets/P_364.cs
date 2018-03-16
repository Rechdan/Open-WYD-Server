using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Visão do MOB - size 232
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct P_364 {
		// Atributos
		public SHeader header;        // 0 a 11			= 12

		public SPosition position;    // 12 a 15		= 4

		public short clientID;        // 16 a 17		= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 12 )]
		public byte [ ] NameBytes;           // 18 a 29		= 12

		public byte chaosPoints;      // 30					= 1
		public byte currentKill;      // 31					= 1
		public short totalKill;       // 32 a 33		= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public short [ ] equip;         // 34 a 65		= 32

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 32 )]
		public SAffectMin [ ] affect;   // 66 a 129		= 64

		public byte guildIndexEFV2;   // 130				= 1
		public byte guildIndexEFV1;   // 131				= 1

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] unk1;           // 132 a 135	= 4

		public SStatus status;        // 136 a 183	= 48

		public EnterVision spawnType; // 184				= 1
		public byte spawnMemberType;  // 185				= 1

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] anct;           // 186 a 201	= 16

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 26 )]
		public byte [ ] TabBytes;            // 202 a 227	= 26

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] unk2;           // 228 a 231	= 4

		// Ajudantes
		public string Name {
			get => Functions.GetString ( this.NameBytes );
			set {
				this.NameBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.NameBytes , 16 );
			}
		}
		public string Tab {
			get => Functions.GetString ( this.TabBytes );
			set {
				this.TabBytes = Config.Encoding.GetBytes ( value );
				Array.Resize ( ref this.TabBytes , 26 );
			}
		}

		// Construtores
		public static P_364 New ( Character character , EnterVision SpawnType ) {
			SMob mob = character.Mob;

			P_364 tmp = new P_364 {
				header = SHeader.New ( 0x364 , Marshal.SizeOf<P_364> ( ) , 30000 ) ,

				position = mob.LastPosition ,

				clientID = mob.ClientId ,

				Name = mob.Name ,

				chaosPoints = mob.ChaosPoints ,
				currentKill = mob.CurrentKill ,
				totalKill = mob.TotalKill ,

				equip = new short [ 16 ] ,

				affect = new SAffectMin [ 32 ] ,

				guildIndexEFV2 = 0 ,
				guildIndexEFV1 = 0 ,

				unk1 = new byte [ 4 ] { 0 , 0 , 0 , 0 } ,

				status = mob.GameStatus ,

				spawnType = SpawnType ,
				spawnMemberType = 0 ,

				anct = new byte [ 16 ] ,

				Tab = mob.Tab ,

				unk2 = new byte [ ] { 0 , 0 , 0 , 0 }
			};

			for ( int i = 0 ; i < tmp.equip.Length ; i++ ) { tmp.equip [ i ] = Functions.GetItemID ( mob.Equip [ i ] , ( i == 14 ) ); }
			for ( int i = 0 ; i < tmp.affect.Length ; i++ ) { tmp.affect [ i ] = SAffectMin.New ( mob.Affects [ i ] ); }
			for ( int i = 0 ; i < tmp.anct.Length ; i++ ) { tmp.anct [ i ] = Functions.GetAnctCode ( mob.Equip [ i ] , ( i == 14 ) ); }

			return tmp;
		}
	}
}