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
		ObjectPooling missileFlame = new ObjectPooling();
		ObjectPooling missileBasic = new ObjectPooling();
		ObjectPooling missileSysrupt = new ObjectPooling();
		ObjectPooling missileAdvanced = new ObjectPooling();
		ObjectPooling missileChromatic = new ObjectPooling();
		ObjectPooling missileShieldBreak = new ObjectPooling();
		#endregion

		#region Lasers
		ObjectPooling laserBasic = new ObjectPooling();
		ObjectPooling laserCharged = new ObjectPooling();
		ObjectPooling miniCannon = new ObjectPooling();
		ObjectPooling cannon = new ObjectPooling();
		//	Explosions
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
			//	Misc
			ammoDrops.Initialize(Resources.Load<GameObject>("AmmoDrop"), 10, myTransform);
			explosionpool.Initialize(Resources.Load<GameObject>("Explosions/EnemyExplosion"), 12, myTransform);
		}
		#endregion

		#region Accessors
		public GameObject InitializeProjectile(ProjectileType _type)
		{
			switch (_type)
			{
				case ProjectileType.BasicMissile:
					missileBasic.Initialize(Resources.Load<GameObject>("Missiles/BasicMissile"), 1, myTransform);
					return missileBasic.GetPooledObject();

				case ProjectileType.AdvancedMissile:
					missileAdvanced.Initialize(Resources.Load<GameObject>("Missiles/AdvancedMissile"), 1, myTransform);
					return missileAdvanced.GetPooledObject();

				case ProjectileType.ShieldBreakMissile:
					missileShieldBreak.Initialize(Resources.Load<GameObject>("Missiles/ShieldbreakMissile"), 1, myTransform);
					return missileShieldBreak.GetPooledObject();

				case ProjectileType.ChromaticMissile:
					missileChromatic.Initialize(Resources.Load<GameObject>("Missiles/ChromaticMissile"), 1, myTransform);
					return missileChromatic.GetPooledObject();

				case ProjectileType.SysruptMissile:
					missileSysrupt.Initialize(Resources.Load<GameObject>("Missiles/SysruptMissile"), 1, myTransform);
					return missileSysrupt.GetPooledObject();

				case ProjectileType.FlameMissile:
					missileFlame.Initialize(Resources.Load<GameObject>("Missiles/FlameMissile"), 1, myTransform);
					return missileFlame.GetPooledObject();

				case ProjectileType.SlowMissile:
					missileSlow.Initialize(Resources.Load<GameObject>("Missiles/SlowMissile"), 1, myTransform);
					return missileSlow.GetPooledObject();

				case ProjectileType.EmpMissile:
					missileEmp.Initialize(Resources.Load<GameObject>("Missiles/EmpMissile"), 1, myTransform);
					return missileEmp.GetPooledObject();


				case ProjectileType.BasicLaser:
					laserBasic.Initialize(Resources.Load<GameObject>("Lasers/BasicLaser"), 1, myTransform);
					return laserBasic.GetPooledObject();

				case ProjectileType.ChargedLaser:
					laserCharged.Initialize(Resources.Load<GameObject>("Lasers/ChargedLaser"), 1, myTransform);
					return laserCharged.GetPooledObject();

				case ProjectileType.MiniCannon:
					miniCannon.Initialize(Resources.Load<GameObject>("Lasers/MiniCannon"), 1, myTransform);
					explosionMiniCannon.Initialize(Resources.Load<GameObject>("Explosions/MiniCannonExplosion"), 1, myTransform);
					return miniCannon.GetPooledObject();

				case ProjectileType.Cannon:
					cannon.Initialize(Resources.Load<GameObject>("Lasers/Cannon"), 1, myTransform);
					explosionCannon.Initialize(Resources.Load<GameObject>("Explosions/BossLaserExplosion"), 1, myTransform);
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
			GameObject go = null;
			switch (_type)
			{
				case ProjectileType.BasicMissile:
					go = missileBasic.GetPooledObject();
					break;
				case ProjectileType.AdvancedMissile:
					go = missileAdvanced.GetPooledObject();
					break;
				case ProjectileType.ShieldBreakMissile:
					go = missileShieldBreak.GetPooledObject();
					break;
				case ProjectileType.ChromaticMissile:
					go = missileChromatic.GetPooledObject();
					break;
				case ProjectileType.SysruptMissile:
					go = missileSysrupt.GetPooledObject();
					break;
				case ProjectileType.FlameMissile:
					go = missileFlame.GetPooledObject();
					break;
				case ProjectileType.SlowMissile:
					go = missileSlow.GetPooledObject();
					break;
				case ProjectileType.EmpMissile:
					go = missileEmp.GetPooledObject();
					break;
				case ProjectileType.MissileEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.BasicLaser:
					go = laserBasic.GetPooledObject();
					break;
				case ProjectileType.ChargedLaser:
					go = laserCharged.GetPooledObject();
					break;
				case ProjectileType.LaserEnd:
					Debug.LogError("Should not be here!");
					break;

				case ProjectileType.MiniCannon:
					go = miniCannon.GetPooledObject();
					break;
				case ProjectileType.Cannon:
					go = cannon.GetPooledObject();
					break;

				default:
					Debug.LogError("Should not be here! " + _type);
					break;
			}

			if (go == null)
				go = InitializeProjectile(_type);

			if (go == null)
				Debug.LogError("WTF?");

			return go;
		}

		public GameObject GetProjectileExplosion(ProjectileType _type)
		{
			switch (_type)
			{
				case ProjectileType.MissileEnd:
					Debug.LogError("Should not be here!");
					break;

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