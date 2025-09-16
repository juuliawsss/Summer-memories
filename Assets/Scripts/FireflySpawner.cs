using System.Collections.Generic;
using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    public GameObject fireflyPrefab;
    public float spawnInterval = 2f;
    public float minDistance = 20f; // Minimum distance between fireflies
    public int maxFireflies = 25;   // Maximum number of fireflies

    private float timer = 0f;
    private List<GameObject> fireflies = new List<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && fireflies.Count < maxFireflies)
        {
            SpawnFirefly();
            timer = 0f;
        }
    }

    void SpawnFirefly()
    {
        Vector3 spawnPosition;
        int attempts = 0;
        bool positionFound = false;

        do
        {
            float x = Random.Range(-70f, 70f);
            float y = transform.position.y;
            float z = Random.Range(-70f, 70f);
            spawnPosition = new Vector3(x, y, z);

            positionFound = true;
            foreach (GameObject firefly in fireflies)
            {
                if (firefly != null && Vector3.Distance(firefly.transform.position, spawnPosition) < minDistance)
                {
                    positionFound = false;
                    break;
                }
            }
            attempts++;
        } while (!positionFound && attempts < 20);

        if (positionFound)
        {
            GameObject newFirefly = Instantiate(fireflyPrefab, spawnPosition, Quaternion.identity);
            fireflies.Add(newFirefly);
        }
    }
}
