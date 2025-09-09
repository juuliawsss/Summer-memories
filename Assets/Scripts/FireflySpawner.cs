using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab;
    public float spawnInterval = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFirefly();
            timer = 0f;
        }
    }

    void SpawnFirefly()
    {
        float x = Random.Range(-70f, 70f); // Change range as needed
        float y = transform.position.y;    // Keep y the same as spawner
        float z = Random.Range(-70f, 70f); // Change range as needed

        Vector3 spawnPosition = new Vector3(x, y, z);
        Instantiate(fireflyPrefab, spawnPosition, Quaternion.identity);
    }
}
