using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AsteroidManager : MonoBehaviour {

    [SerializeField] GameObject[] asteroidPrefab;
    [SerializeField] Transform spawnPt;
    [SerializeField] Vector3 bounds;

    [SerializeField] int maxAsteroids =50;
    [SerializeField] int numAsteroids;

    private BoxCollider boxCol;

    private void Awake()
    {
        numAsteroids = 0;
        boxCol = GetComponent<BoxCollider>();
        bounds = new Vector3(boxCol.size.x * .5f, boxCol.size.y * .5f, boxCol.size.z * .5f);

        if (asteroidPrefab.Length == 0)
            Debug.LogError("Asteroid Manager's prefab list is empty");
    }

    private void FixedUpdate()
    {
        while (maxAsteroids > numAsteroids)
            PlaceAsteroid();
    }

    void PlaceAsteroid()
    {
        GameObject go = Instantiate(asteroidPrefab[Random.Range(0, asteroidPrefab.Length)],
                        Vector3.zero, Quaternion.identity) as GameObject;

        float x = Random.Range(-bounds.x + boxCol.center.x, bounds.x + boxCol.center.x);
        float y = Random.Range(-bounds.y + boxCol.center.y, bounds.y + boxCol.center.y);
        float z = Random.Range(-bounds.z + boxCol.center.z, bounds.z + boxCol.center.z);

        go.transform.parent = spawnPt.transform;
        go.transform.localPosition = new Vector3(x, y, z);
        numAsteroids++;
    }

    public void DestroyAsteroid()
    {
        numAsteroids--;
    }
}
