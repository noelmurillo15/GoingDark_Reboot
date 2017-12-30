using UnityEngine;

public class Asteroid : MonoBehaviour
{

    #region Properties
    [SerializeField] private GameObject supplybox;

    [SerializeField] float minScale = 1f;
    [SerializeField] float maxScale = 5f;
    [SerializeField] float velocityOffset = 30f;
    [SerializeField] float rotationOffset = 15f;

    private BoxCollider boxcol;
    private Rigidbody MyRigidbody;
    private Transform MyTransform;

    private bool skipStart;
    private Vector3 m_velocity;
    private Vector3 m_rotation;    
    #endregion


    private void Awake()
    {
        MyTransform = transform;
        boxcol = GetComponent<BoxCollider>();
        MyRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {        
        if (!skipStart)
        {
            //   Velocity
            m_velocity.x = Random.Range(-velocityOffset, velocityOffset);
            m_velocity.y = Random.Range(-velocityOffset, velocityOffset);
            m_velocity.z = Random.Range(-velocityOffset, velocityOffset);

            //   Rotation
            m_rotation.x = Random.Range(-rotationOffset, rotationOffset);
            m_rotation.y = Random.Range(-rotationOffset, rotationOffset);
            m_rotation.z = Random.Range(-rotationOffset, rotationOffset);

            //   Scale
            float m_scale = 1f;
            m_scale = Random.Range(minScale, maxScale);
            
            Vector3 newScale = MyTransform.localScale;
            newScale.x *= m_scale;
            newScale.y *= m_scale;
            newScale.z *= m_scale;
            MyTransform.localScale = newScale;
        }
        else
        {
            //  Change Properties for asteroid shatter peices
            m_velocity.x = Random.Range(-150.0f, 150.0f);
            m_velocity.y = Random.Range(-150.0f, 150.0f);
            m_velocity.z = Random.Range(-150.0f, 150.0f);

            m_rotation.x = Random.Range(1.0f, 359.0f);
            m_rotation.y = Random.Range(1.0f, 359.0f);
            m_rotation.z = Random.Range(1.0f, 359.0f);

            MyTransform.localEulerAngles = m_rotation;
            MyTransform.localScale *= Random.Range(.2f, .55f);

            m_rotation.x = Random.Range(-rotationOffset, rotationOffset);
            m_rotation.y = Random.Range(-rotationOffset, rotationOffset);
            m_rotation.z = Random.Range(-rotationOffset, rotationOffset);

            Invoke("EnableCollision", .25f);
            Invoke("SelfDestruct", 10f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_rotation * Time.fixedDeltaTime);
        MyRigidbody.MoveRotation(MyRigidbody.rotation * deltaRotation);
        MyRigidbody.MovePosition(MyRigidbody.position + (m_velocity * Time.fixedDeltaTime));
    }

    void OnBecameVisible()
    {        
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void EnableCollision()
    {
        boxcol.enabled = true;
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void Kill()
    {
        if (MyTransform.parent == null)
        {
            Debug.LogError("Instantiated Asteroid is not under parent environment");
            return;
        }

        skipStart = true;
        boxcol.enabled = false;
        AudioManager.instance.PlayAstDestroy();
        float range = Random.Range(3, 6);
        for (int i = 0; i < range; i++)
        {
            GameObject go = Instantiate(gameObject) as GameObject;
            go.transform.parent = MyTransform.parent;
        }

        if(supplybox != null)
        {
            GameObject go = Instantiate(supplybox, MyTransform.position, Quaternion.identity) as GameObject;
            go.transform.parent = MyTransform.parent;
        }
        else
            Debug.LogError("Asteroid does not have supplyBox attached");
        

        SelfDestruct();
    }    
}