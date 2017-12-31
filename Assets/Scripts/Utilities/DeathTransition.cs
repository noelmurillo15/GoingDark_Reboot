using UnityEngine;

public class DeathTransition : MonoBehaviour
{
    private bool isDead;
    public bool spawn;
    public bool flip;
    //   TODO : add vignette for death screen transition
    // Use this for initialization
    void Start()
    {
        spawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || spawn)
        {

        }
        else
        {

        }
    }

    public void Death()
    {
        isDead = true;
    }

    public void SpawnPlayer()
    {
        spawn = true;
    }

    public void NotDead()
    {
        isDead = false;
    }

    public void notSpawned()
    {
        spawn = false;
    }
}