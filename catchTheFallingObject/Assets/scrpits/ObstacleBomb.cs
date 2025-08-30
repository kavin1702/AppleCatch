using UnityEngine;

public class ObstacleBomb : MonoBehaviour
{
    public GameObject fallingObjectPrefab;                                    // The object to spawn
    public float spawnInterval = 3.5f;                                       // Time between each spawn
    public float spawnRangeX = 8f;                                          // How far left/right objects can spawn
 
    void Start()
    {
        // Call the SpawnObject method repeatedly every few seconds
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    void SpawnObject()
    {
        // Generate a random X position within the given range
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);

        // Create a new falling object at this position
        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
       
    }
    
}
