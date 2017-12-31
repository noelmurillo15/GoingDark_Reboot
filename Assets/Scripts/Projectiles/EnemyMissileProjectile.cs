using UnityEngine;
using GoingDark.Core.Enums;

public class EnemyMissileProjectile : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public EnemyMissileType Type;
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float rotateSpeed = 0f;

    private bool init = false;
    private bool tracking;
    private float baseDmg;
    private Transform target;
    private Quaternion targetRotation;

    private string hitfunc;

    private Transform MyTransform;
    private Rigidbody MyRigidbody;
    private MessageScript messages;
    private ObjectPoolManager poolManager;
    #endregion


    public void OnEnable()
    {
        if (!init)
        {
            init = true;
            tracking = false;
            targetRotation = Quaternion.identity;            

            switch (Type)
            {
                case EnemyMissileType.Basic:
                    hitfunc = "MissileDmg";
                    baseDmg = 5f;
                    break;
                case EnemyMissileType.Slow:
                    hitfunc = "MissileDebuff";
                    baseDmg = 2f;
                    break;
                case EnemyMissileType.Emp:
                    hitfunc = "MissileDebuff";
                    baseDmg = 2f;
                    break;
                case EnemyMissileType.Guided:
                    hitfunc = "MissileDmg";
                    baseDmg = 8f;
                    break;
                case EnemyMissileType.Sysrupt:
                    hitfunc = "MissileDebuff";
                    baseDmg = 2f;
                    break;
                case EnemyMissileType.Nuke:
                    hitfunc = "MissileDmg";
                    baseDmg = 20f;
                    break;
                case EnemyMissileType.ShieldBreak:
                    hitfunc = "MissileDebuff";
                    baseDmg = 33.66f;
                    break;
            }            

            MyTransform = transform;
            MyRigidbody = GetComponent<Rigidbody>();
            messages = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
            poolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();
            gameObject.SetActive(false);
        }
        else
        {           
            tracking = false;            
            Invoke("InitializeTracking", 1f);
            Invoke("Kill", 3f);
        }
    }

    void FixedUpdate()
    {
        if (tracking)
        {
            LookAt();
            MyTransform.Translate(0f, 0f, speed * Time.fixedDeltaTime);
        }
        else
            MyRigidbody.MovePosition(MyTransform.position + MyTransform.forward * Time.fixedDeltaTime * speed);
    }

    #region Accessors
    public float GetBaseDamage()
    {
        return baseDmg;
    }
    #endregion

    #region Tracking
    void InitializeTracking()
    {
        float rand = Random.Range(1f, 100f);
        if(rand >= 66.66f)
        {
            messages.MissileIncoming();
            tracking = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (target == null)
            {
                Debug.LogError("Enemy Missile Target was not found");
                tracking = false;
            }
        }
    }
    void LookAt()
    {
        if (target != null)
        {
            targetRotation = Quaternion.LookRotation(target.position - MyTransform.position);
            MyTransform.rotation = Quaternion.Slerp(MyTransform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
        }
    }    
    #endregion

    #region Recycle Death
    void SetInactive()
    {
        tracking = false;
        gameObject.SetActive(false);
    }

    public void Kill()
    {
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        GameObject go = poolManager.GetEnemyExplosion();

        if (go != null)
        {
            go.transform.position = MyTransform.position;
            go.SetActive(true);
        }

        SetInactive();
    }
    #endregion

    #region Collision
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Player")
        {
            if (IsInvoking("Kill"))
                CancelInvoke("Kill");

            col.transform.SendMessage(hitfunc, this);
        }
        else if (col.transform.tag == "Asteroid" || col.transform.tag == "Decoy")
        {
            col.gameObject.SendMessage("Kill");
            Kill();
        }
    }
    #endregion
}