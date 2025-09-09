using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab;
    public float spawnInterval = 7f;

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
        Vector3 spawnPosition = transform.position;
        Instantiate(fireflyPrefab, spawnPosition, Quaternion.identity);
    }
}
