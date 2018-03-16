using System;
using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura do mob - size 1336
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SMob {
		// Atributos
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] NameBytes;      // 0 a 15				= 16

		public byte CapeInfo;           // 16						= 1
		public byte Merchant;           // 17						= 1

		public short GuildIndex;        // 18 a 19			= 2
		public byte ClassInfo;          // 20						= 1
		public byte AffectInfo;         // 21						= 1
		public short QuestInfo;         // 22 a 23			= 2

		public int Gold;                // 24 a 27			= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] Unk1;           // 28 a 31			= 4

		public ulong Exp;               // 32 a 39			= 8

		public SPosition LastPosition;  // 40 a 43			= 4

		public SStatus BaseStatus;      // 44 a 91			= 48
		public SStatus GameStatus;      // 92 a 139			= 48

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public SItem [ ] Equip;         // 140 a 267		= 128
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 60 )]
		public SItem [ ] Inventory;     // 268 a 747		= 480
		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public SItem [ ] Andarilho;     // 748 a 763		= 16

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 8 )]
		public byte [ ] Unk2;           // 764 a 771		= 8

		public short Item547;           // 772 a 773		= 2
		public byte ChaosPoints;        // 774					= 1
		public byte CurrentKill;        // 775					= 1
		public short TotalKill;         // 776 a 777		= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public byte [ ] Unk3;           // 778 a 779		= 2

		public ulong Learn;             // 780 a 787		= 8

		public short StatusPoint;       // 788 a 789		= 2
		public short MasterPoint;       // 790 a 791		= 2
		public short SkillPoint;        // 792 a 793		= 2

		public byte Critical;           // 794					= 1
		public byte SaveMana;           // 795					= 1

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] SkillBar1;      // 796 a 799		= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 4 )]
		public byte [ ] Unk4;           // 800 a 803		= 4

		public byte ResistFire;         // 804					= 1
		public byte ResistIce;          // 805					= 1
		public byte ResistHoly;         // 806					= 1
		public byte ResistThunder;      // 807					= 1

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 210 )]
		public byte [ ] Unk5;           // 808 a 1017		= 210

		public short MagicIncrement;    // 1018 a 1019	= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 6 )]
		public byte [ ] Unk6;           // 1020 a 1025	= 6

		public short ClientId;          // 1026 a 1027	= 2
		public short CityId;            // 1028 a 1029	= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 16 )]
		public byte [ ] SkillBar2;      // 1030 a 1045	= 16

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public byte [ ] Unk7;           // 1046 a 1047	= 2

		public uint Hold;               // 1048 a 1051	= 4

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 26 )]
		public byte [ ] TabBytes;       // 1052 a 1077	= 26

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 2 )]
		public byte [ ] Unk8;           // 1078 a 1079	= 2

		[MarshalAs ( UnmanagedType.ByValArray , SizeConst = 32 )]
		public SAffect [ ] Affects;     // 1080 a 1335	= 256

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
		public static SMob New ( ) {
			SMob tmp = new SMob {
				Name = "" ,

				CapeInfo = 0 ,
				Merchant = 0 ,

				GuildIndex = 0 ,
				ClassInfo = 0 ,
				AffectInfo = 0 ,
				QuestInfo = 0 ,

				Gold = 0 ,

				Unk1 = new byte [ 4 ] ,

				Exp = 0 ,

				LastPosition = SPosition.New ( ) ,

				BaseStatus = SStatus.New ( ) ,
				GameStatus = SStatus.New ( ) ,

				Equip = new SItem [ 16 ] ,
				Inventory = new SItem [ 60 ] ,
				Andarilho = new SItem [ 2 ] ,

				Unk2 = new byte [ 8 ] ,

				Item547 = 0 ,
				ChaosPoints = 0 ,
				CurrentKill = 0 ,
				TotalKill = 0 ,

				Unk3 = new byte [ 2 ] ,

				Learn = 0 ,

				StatusPoint = 0 ,
				MasterPoint = 0 ,
				SkillPoint = 0 ,

				Critical = 0 ,
				SaveMana = 0 ,

				SkillBar1 = new byte [ 4 ] ,

				Unk4 = new byte [ 4 ] ,

				ResistHoly = 0 ,
				ResistThunder = 0 ,
				ResistFire = 0 ,
				ResistIce = 0 ,

				Unk5 = new byte [ 210 ] ,

				MagicIncrement = 0 ,

				Unk6 = new byte [ 6 ] ,

				ClientId = 0 ,
				CityId = 0 ,

				SkillBar2 = new byte [ 16 ] ,

				Unk7 = new byte [ 2 ] { 204 , 204 } ,

				Hold = 0 ,

				Tab = "" ,

				Unk8 = new byte [ 2 ] ,

				Affects = new SAffect [ 32 ]
			};

			for ( int i = 0 ; i < tmp.Equip.Length ; i++ ) { tmp.Equip [ i ] = SItem.New ( ); }
			for ( int i = 0 ; i < tmp.Inventory.Length ; i++ ) { tmp.Inventory [ i ] = SItem.New ( ); }
			for ( int i = 0 ; i < tmp.Andarilho.Length ; i++ ) { tmp.Andarilho [ i ] = SItem.New ( ); }

			for ( int i = 0 ; i < tmp.SkillBar1.Length ; i++ ) { tmp.SkillBar1 [ i ] = 255; }
			for ( int i = 0 ; i < tmp.SkillBar2.Length ; i++ ) { tmp.SkillBar2 [ i ] = 255; }

			for ( int i = 0 ; i < tmp.Affects.Length ; i++ ) { tmp.Affects [ i ] = SAffect.New ( ); }

			return tmp;
		}

		private static SMob Base ( string name ) {
			SMob mob = New ( );

			mob.Name = name;

			mob.BaseStatus.Defense = 4;
			mob.BaseStatus.MobSpeed = 6;

			mob.Equip [ 0 ] = SItem.New ( 0 , 43 , 0 , 86 , 1 , 28 , 0 );

			//mob.Inventory.AddItemToCharacter ( SItem.New ( 685 , 61 , 20 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 691 , 61 , 20 ) );

			mob.Item547 = 547;
			mob.ChaosPoints = 150;
			mob.CurrentKill = 0;
			mob.TotalKill = 0;

			mob.LastPosition = SPosition.New ( 2100 , 2100 );

			mob.GameStatus = mob.BaseStatus;

			return mob;
		}

		public static SMob TK ( string name ) {
			SMob mob = Base ( name );

			mob.ClassInfo = 0;

			mob.Equip [ 0 ].Id = 1;
			mob.Equip [ 1 ] = SItem.New ( 1103 );
			mob.Equip [ 2 ] = SItem.New ( 1115 );
			mob.Equip [ 3 ] = SItem.New ( 1127 );
			mob.Equip [ 4 ] = SItem.New ( 1139 );
			mob.Equip [ 5 ] = SItem.New ( 1151 );

			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 941 , 43 , 6 , 60 , 24 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 942 , 43 , 6 , 2 , 54 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 945 , 43 , 6 , 2 , 54 ) );

			return mob;
		}
		public static SMob FM ( string name ) {
			SMob mob = Base ( name );

			mob.ClassInfo = 1;

			mob.Equip [ 0 ].Id = 11;
			mob.Equip [ 1 ] = SItem.New ( 1253 );
			mob.Equip [ 2 ] = SItem.New ( 1265 );
			mob.Equip [ 3 ] = SItem.New ( 1277 );
			mob.Equip [ 4 ] = SItem.New ( 1289 );
			mob.Equip [ 5 ] = SItem.New ( 1301 );

			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 940 , 43 , 6 , 60 , 24 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 943 , 43 , 6 , 2 , 54 ) );

			return mob;
		}
		public static SMob BM ( string name ) {
			SMob mob = Base ( name );

			mob.ClassInfo = 2;

			mob.Equip [ 0 ].Id = 21;
			mob.Equip [ 1 ] = SItem.New ( 1403 );
			mob.Equip [ 2 ] = SItem.New ( 1406 );
			mob.Equip [ 3 ] = SItem.New ( 1409 );
			mob.Equip [ 4 ] = SItem.New ( 1412 );
			mob.Equip [ 5 ] = SItem.New ( 1415 );

			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 941 , 43 , 6 , 60 , 24 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 942 , 43 , 6 , 2 , 54 ) );

			return mob;
		}
		public static SMob HT ( string name ) {
			SMob mob = Base ( name );

			mob.ClassInfo = 3;

			mob.Equip [ 0 ].Id = 31;
			mob.Equip [ 1 ] = SItem.New ( 1553 );
			mob.Equip [ 2 ] = SItem.New ( 1556 );
			mob.Equip [ 3 ] = SItem.New ( 1559 );
			mob.Equip [ 4 ] = SItem.New ( 1562 );
			mob.Equip [ 5 ] = SItem.New ( 1565 );

			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 938 , 43 , 6 , 2 , 45 ) );
			//mob.Inventory.AddItemToCharacter ( SItem.New ( 943 , 43 , 6 , 2 , 54 ) );

			return mob;
		}
	}
}