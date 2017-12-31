namespace GoingDark.Core.Enums
{
    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

    #region Debuffs
    public enum Impairments
    {
        None,
        Slowed,
        Stunned,
    }
    #endregion

    #region Store
    public enum Items
    {
        BasicMissile,
        ShieldBreakMissile,
        ChromaticMissile,
        EMPMissile,
        LaserPowerUpgrade,
        Laser2PowerUpgrade,
        HealthUpgrade,
        ShieldUpgrade,
    }

    public struct Item
    {
        public string ItemName;
        public int ItemPrice;
        public Items Type;
    }
    #endregion

    #region Projectiles
    public enum MissileType
    {
        Basic,
        Emp,
        ShieldBreak,
        Chromatic,
        NumberOfType        
    }
    public enum LaserType
    {
        Basic,
        Charged,
        NumberOfType
    }
    #endregion

    #region Enemies
    public enum EnemyMissileType
    {
        Basic,
        Slow,
        Emp,
        Guided,
        Sysrupt,
        Nuke,
        ShieldBreak
    }
    public enum EnemyLaserType
    {
        Basic,
        Charged,
        MiniCannon,
        Cannon
    }
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