///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>


namespace GoingDark.Core.Enums {


    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

	#region Enemies
	public enum EnemyTypes
    {
        None,
        Basic,
        Droid,
        SquadLead,
        JetFighter,
        Transport,
        Trident,
        Tank,
        Any,
        FinalBoss
    };
    public enum EnemyStates
    {
        Idle,
        Patrol,
        Alert,
        Follow,
        Flee,
        Puzzled,
        Attack,
    }    
    #endregion

    #region Debuffs
    public enum Impairments
    {
        None,
        Slowed,
        Stunned,
		Enflamed,
		Sysrupt,
		ShieldBreak,
    }
    #endregion

    #region Projectiles
	public enum ProjectileType
	{
		//	Missiles
		BasicMissile,
		AdvancedMissile,
		ShieldBreakMissile,
		ChromaticMissile,
		SysruptMissile,
		FlameMissile,
		SlowMissile,
		EmpMissile,
		MissileEnd,

		//	Laser
		BasicLaser,
		ChargedLaser,
		LaserEnd,

		//	Boss
		MiniCannon,
		Cannon,
	}
	#endregion

    #region Ship Systems
    public enum SystemType
    {
        None,
        Cloak,
        Decoy,
        Laser,
        Shield,
        Missile,
        Hyperdrive
    }   
    public enum SystemStatus
    {
        Offline,
        Online,
        Damaged
    }
    #endregion
}