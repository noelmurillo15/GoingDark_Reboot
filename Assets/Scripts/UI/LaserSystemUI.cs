using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;


public class LaserSystemUI : MonoBehaviour {

	Text typeTxt;

	// Use this for initialization
	void Start () {
		typeTxt = GameObject.Find("LaserChoice").GetComponent<Text>();
		typeTxt.text = "BasicLaser";
		typeTxt.color = Color.cyan;
	}

	public void LaserSwap(LaserSystem.Laser laser)
	{
		switch (laser.type)
		{
			case ProjectileType.BasicLaser:
				typeTxt.text = "Basic";
				typeTxt.color = Color.gray;
				break;
			case ProjectileType.ChargedLaser:
				typeTxt.text = "Charged";
				typeTxt.color = Color.cyan;
				break;
			
		}
	}
}