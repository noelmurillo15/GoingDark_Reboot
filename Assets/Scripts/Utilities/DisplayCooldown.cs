using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using System.Collections;

public class DisplayCooldown : MonoBehaviour {

    public SystemType Type;
    private Text text;
    private SystemManager system;


	void Start () {
        system = GameObject.Find("Devices").GetComponent<SystemManager>();
        text = GetComponent<Text>();

       UpdateCooldowns();
	}

    #region Coroutine
    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return new WaitForSeconds(1);
        }
    }
    private void CooldownCheck()
    {
        int num = system.GetSystemCooldown(Type);
        if (num == -1)
        {
            text.color = Color.grey;
            text.text = "System Unavailable";
        }
        else if (num == -10)
        {
            text.color = Color.grey;
            text.text = "System Offline";
        }
        else if (num == 0)
        {
            text.color = Color.green;
            text.text = "System Ready";
        }
        else if(num == -2)
        {
            text.color = Color.cyan;
            text.text = "Active";
        }
        else
        {
            text.color = Color.red;
            text.text = num.ToString();
        }
    }
    #endregion
}
