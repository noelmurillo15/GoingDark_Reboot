///<summary>
/// 3/7/2018
/// Allan Noel Murillo
/// GoingDark_Reboot
/// </summary>
using UnityEngine;
using GoingDark.Core.Enums;

namespace ANM.Utilities {

	public class ObjectPoolManager : MonoBehaviour {


		#region Singleton Design Pattern
		private static ObjectPoolManager objectPoolManager = null;

		public static ObjectPoolManager Instance
		{
			get
			{
				if (objectPoolManager == null)
				{
					Debug.Log("No instance of ObjectPoolManager currently exists");
					return null;
				}
				return objectPoolManager;
			}
		}
		#endregion

		#region References
		Transform myTransform;
		#endregion

		#region Missiles
		ObjectPooling missileEmp = new ObjectPooling();
		ObjectPooling missileSlow = new ObjectPooling();
		//ObjectPooling missileNuke = new ObjectPooling();
		ObjectPooling missileFlame = new ObjectPooling();
		ObjectPooling missileBasic = new ObjectPooling();
		ObjectPooling missileSysrupt = new ObjectPooling();
		ObjectPooling missileAdvanced = new ObjectPooling();
		ObjectPooling missileChromatic = new ObjectPooling();
		ObjectPooling missileShieldBreak = new ObjectPooling();
		//  Explosions
		ObjectPooling explosionEmpMissile = new ObjectPooling();
		ObjectPooling explosionBasicMissile = new ObjectPooling();
		ObjectPooling explosionChromaticMissile = new ObjectPooling();
		ObjectPooling explosionShieldBreakMissile = new ObjectPooling();
		#endregion

		#region Lasers
		ObjectPooling laserBasic = new ObjectPooling();
		ObjectPooling laserCharged = new ObjectPooling();
		ObjectPooling miniCannon = new ObjectPooling();
		ObjectPooling cannon = new ObjectPooling();
		//	Explosions
		ObjectPooling explosionBasicLaser = new ObjectPooling();
		ObjectPooling explosionChargedLaser = new ObjectPooling();
		ObjectPooling explosionMiniCannon = new ObjectPooling();
		ObjectPooling explosionCannon = new ObjectPooling();
		#endregion

		#region Misc
		ObjectPooling ammoDrops = new ObjectPooling();
		ObjectPooling explosionpool = new ObjectPooling();
		#endregion



		#region Unity Functions
		void Awake()
		{
			//	Signleton Design Pattern
			if (objectPoolManager == null)
			{
				objectPoolManager = this;
				DontDestroyOnLoad(objectPoolManager);
			}
			else
			{
				DestroyImmediate(this);
				return;
			}
			//	References
			myTransform = transform;
			//	Missiles
			missileEmp.Initialize(Resources.Load<GameObject>("Missiles/Emp"), 6, myTransform);
			missileSlow.Initialize(Resources.Load<GameObject>("Missiles/Slow"), 6, myTransform);
			missileBasic.Initialize(Resources.Load<GameObject>("Missiles/Basic"), 6, myTransform);
			missileFlame.Initialize(Resources.Load<GameObject>("Missiles/Flame"), 6, myTransform);
			missileSysrupt.Initialize(Resources.Load<GameObject>("Missiles/Sysrupt"), 6, myTransform);
			missileAdvanced.Initialize(Resources.Load<GameObject>("Missiles/Advanced"), 6, myTransform);
			missileChromatic.Initialize(Resources.Load<GameObject>("Missiles/Chromatic"), 6, myTransform);
			missileShieldBreak.Initialize(Resources.Load<GameObject>("Missiles/ShieldBreak"), 6, myTransform);
			//	Missile Explosions
			explosionEmpMissile.Initialize(Resources.Load<GameObject>("Explosions/EmpExplosion"), 6, myTransform);
			explosionBasicMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 6, myTransform);
			explosionChromaticMissile.Initialize(Resources.Load<GameObject>("Explosions/ChromaticExplosion"), 6, myTransform);
			explosionShieldBreakMissile.Initialize(Resources.Load<GameObject>("Explosions/ShieldBreakExplosion"), 6, myTransform);
			//	Lasers
			laserBasic.Initialize(Resources.Load<GameObject>("Lasers/Basic"), 12, myTransform);
			laserCharged.Initialize(Resources.Load<GameObject>("Lasers/Charged"), 12, myTransform);
			miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniCannon"), 12, myTransform);
			cannon.Initialize(Resources.Load<GameObject>("Lasers/Cannon"), 12, myTransform);
			//	Laser Explosions
			explosionBasicLaser.Initialize(Resources.Load<GameObject>("Explosions/LaserExplosion"), 12, myTransform);
			explosionChargedLaser.Initialize(Resources.Load<GameObject>("Explosions/ChargedLaserExplosion"), 12, myTransform);
			explosionMiniCannon.Initialize(Resources.Load<GameObject>("Explosions/MiniCannonExplosion"), 12, myTransform);
			explosionCannon.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplosion"), 12, myTransform);
			//	Misc
			ammoDrops.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, myTransform);
			explosionpool.Initialize(Resources.Load<GameObject>("Explosions/EnemyExplosion"), 12, myTransform);
		}
		#endregion

		#region Accessors
		public GameObject InitializeProjectile(ProjectileType _type)
		{
			Debug.Log("Not enough Projectiles of type : " + _type + "... creating more in object pool.");
			switch (_type)
			{
				case ProjectileType.BasicMissile:
					missileBasic.Initialize(Resources.Load<GameObject>("Missiles/Basic"), 3, myTransform);
					explosionBasicMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 3, myTransform);
					return missileBasic.GetPooledObject();

				case ProjectileType.AdvancedMissile:
					missileAdvanced.Initialize(Resources.Load<GameObject>("Missiles/Advanced"), 3, myTransform);
					explosionBasicMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 3, myTransform);
					return missileAdvanced.GetPooledObject();

				case ProjectileType.ShieldBreakMissile:
					missileShieldBreak.Initialize(Resources.Load<GameObject>("Missiles/ShieldBreak"), 3, myTransform);
					explosionShieldBreakMissile.Initialize(Resources.Load<GameObject>("Explosions/ShieldBreakExplosion"), 3, myTransform);
					return missileShieldBreak.GetPooledObject();

				case ProjectileType.ChromaticMissile:
					missileChromatic.Initialize(Resources.Load<GameObject>("Missiles/Chromatic"), 3, myTransform);
					explosionChromaticMissile.Initialize(Resources.Load<GameObject>("Explosions/ChromaticExplosion"), 3, myTransform);
					return missileChromatic.GetPooledObject();

				case ProjectileType.SysruptMissile:
					missileSysrupt.Initialize(Resources.Load<GameObject>("Missiles/Sysrupt"), 3, myTransform);
					explosionShieldBreakMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 3, myTransform);
					return missileSysrupt.GetPooledObject();

				case ProjectileType.FlameMissile:
					missileFlame.Initialize(Resources.Load<GameObject>("Missiles/Flame"), 3, myTransform);
					explosionBasicMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 3, myTransform);
					return missileFlame.GetPooledObject();

				case ProjectileType.SlowMissile:
					missileSlow.Initialize(Resources.Load<GameObject>("Missiles/Slow"), 3, myTransform);
					explosionBasicMissile.Initialize(Resources.Load<GameObject>("Explosions/BasicMissileExplosion"), 3, myTransform);
					return missileSlow.GetPooledObject();

				case ProjectileType.EmpMissile:
					missileEmp.Initialize(Resources.Load<GameObject>("Missiles/Emp"), 3, myTransform);
					explosionEmpMissile.Initialize(Resources.Load<GameObject>("Explosions/EmpExplosion"), 3, myTransform);
					return missileEmp.GetPooledObject();


				case ProjectileType.BasicLaser:
					laserBasic.Initialize(Resources.Load<GameObject>("Lasers/LaserBeam"), 10, myTransform);
					explosionBasicLaser.Initialize(Resources.Load<GameObject>("Explosions/LaserExplosion"), 10, myTransform);
					return laserBasic.GetPooledObject();

				case ProjectileType.ChargedLaser:
					laserCharged.Initialize(Resources.Load<GameObject>("Lasers/ChargedShot"), 10, myTransform);
					explosionChargedLaser.Initialize(Resources.Load<GameObject>("Explosions/ChargedLaserExplosion"), 10, myTransform);
					return laserCharged.GetPooledObject();

				case ProjectileType.MiniCannon:
					miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniBossLaser"), 5, myTransform);
					explosionMiniCannon.Initialize(Resources.Load<GameObject>("Explosions/MiniCannonExplosion"), 5, myTransform);
					return miniCannon.GetPooledObject();

				case ProjectileType.Cannon:
					cannon.Initialize(Resources.Load<GameObject>("Lasers/BossLaser"), 3, myTransform);
					explosionCannon.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplosion"), 3, myTransform);
					return cannon.GetPooledObject();

				default:
					Debug.LogError("Should not be here! " + _type);
					break;
			}
			Debug.LogError("Should not reach this point! " + _type);
			return null;
		}

		public GameObject GetProjectile(ProjectileType _type)
		{
			switch (_type)
			{
				case ProjectileType.BasicMissile:
					return missileBasic.GetPooledObject();
				case ProjectileType.AdvancedMissile:
					return missileAdvanced.GetPooledObject();
				case ProjectileType.ShieldBreakMissile:
					return missileShieldBreak.GetPooledObject();
				case ProjectileType.ChromaticMissile:
					return missileChromatic.GetPooledObject();
				case ProjectileType.SysruptMissile:
					return missileSysrupt.GetPooledObject();
				case ProjectileType.FlameMissile:
					return missileFlame.GetPooledObject();
				case ProjectileType.SlowMissile:
					return missileSlow.GetPooledObject();
				case ProjectileType.EmpMissile:
					return missileEmp.GetPooledObject();
				case ProjectileType.MissileEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.BasicLaser:
					return laserBasic.GetPooledObject();
				case ProjectileType.ChargedLaser:
					return laserCharged.GetPooledObject();
				case ProjectileType.LaserEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.MiniCannon:
					return miniCannon.GetPooledObject();
				case ProjectileType.Cannon:
					return cannon.GetPooledObject();

				default:
					Debug.LogError("Should not be here! " + _type);
					break;
			}
			Debug.LogError("Should not reach this point!");
			return null;
		}

		public GameObject GetProjectileExplosion(ProjectileType _type)
		{
			switch (_type)
			{
				case ProjectileType.BasicMissile:
					return explosionBasicMissile.GetPooledObject();
				case ProjectileType.AdvancedMissile:
					return explosionBasicMissile.GetPooledObject();
				case ProjectileType.ShieldBreakMissile:
					return explosionShieldBreakMissile.GetPooledObject();
				case ProjectileType.ChromaticMissile:
					return explosionChromaticMissile.GetPooledObject();
				case ProjectileType.SysruptMissile:
					return explosionBasicMissile.GetPooledObject();
				case ProjectileType.FlameMissile:
					return explosionBasicMissile.GetPooledObject();
				case ProjectileType.SlowMissile:
					return explosionBasicLaser.GetPooledObject();
				case ProjectileType.EmpMissile:
					return explosionEmpMissile.GetPooledObject();
				case ProjectileType.MissileEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.BasicLaser:
					return explosionBasicLaser.GetPooledObject();
				case ProjectileType.ChargedLaser:
					return explosionChargedLaser.GetPooledObject();
				case ProjectileType.LaserEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.MiniCannon:
					return explosionMiniCannon.GetPooledObject();
				case ProjectileType.Cannon:
					return explosionCannon.GetPooledObject();

				default:
					Debug.LogError("Should not be here! " + _type);
					break;
			}
			return null;
		}

		public GameObject GetEnemyExplosion()
		{
			return explosionpool.GetPooledObject();
		}

		public GameObject GetAmmoDrop()
		{
			return ammoDrops.GetPooledObject();
		}
		#endregion
	}
}