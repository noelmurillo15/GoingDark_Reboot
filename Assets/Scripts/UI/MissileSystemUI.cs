using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;


public class MissileSystemUI : MonoBehaviour {


	//  Missile Display
	[SerializeField] Text typeTxt;
	[SerializeField] Text countTxt;
	[SerializeField] Image missileSprite;



	void Start () {
		typeTxt.text = "Basic";
		typeTxt.color = Color.yellow;
		countTxt.color = Color.yellow;
		missileSprite.color = Color.yellow;
	}

	public void UpdateUI(MissileSystem.Missile miss)
	{
		if (miss.count == 0)
		{
			countTxt.text = "";
			typeTxt.text = miss.type.ToString();
			typeTxt.color = Color.grey;
			countTxt.color = Color.grey;
			missileSprite.color = Color.grey;
			return;
		}
		else
		{
			countTxt.text = "x : " + miss.count;
		}
	}

	public void MissileSwap(MissileSystem.Missile miss)
	{
		switch (miss.type)
		{
			case ProjectileType.BasicMissile:
				typeTxt.text = "Basic";
				typeTxt.color = Color.gray;
				break;
			case ProjectileType.EmpMissile:
				typeTxt.text = "Emp";
				typeTxt.color = Color.cyan;
				break;
			case ProjectileType.ShieldBreakMissile:
				typeTxt.text = "ShieldBreak";
				typeTxt.color = Color.magenta;
				break;
			case ProjectileType.ChromaticMissile:
				typeTxt.text = "Chromatic";
				typeTxt.color = Color.green;
				break;
			case ProjectileType.SlowMissile:
				typeTxt.text = "Slow";
				typeTxt.color = Color.blue;
				break;
			case ProjectileType.SysruptMissile:
				typeTxt.text = "Sysrupt";
				typeTxt.color = Color.yellow;
				break;
			case ProjectileType.AdvancedMissile:
				typeTxt.text = "Advanced";
				typeTxt.color = Color.white;
				break;
			case ProjectileType.FlameMissile:
				typeTxt.text = "Flame";
				typeTxt.color = Color.red;
				break;
		}
		countTxt.color = typeTxt.color;
		missileSprite.color = typeTxt.color;
		UpdateUI(miss);
	}
}