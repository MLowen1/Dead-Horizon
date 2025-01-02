using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public GameObject[] EnemyPrefab;
    public bool IsSpawning = true;
    private float radius;

    [Header("Spawn time")]
    [SerializeField] private float minspawntime = 1;
    [SerializeField] private float maxspawntime = 5;

    // Coroutine to spawn asteroids at random intervals
    private IEnumerator randomSpawner()
    {
        while (IsSpawning)
        {
            SpawnEnemy();

            // Wait for a random amount of time before spawning the next asteroid
            float spawnInterval = Random.Range(minspawntime, maxspawntime);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Start()
    {
        //here we dynamically set the radius to be that of the black hole
        BlackHole_Grav blackHole = FindObjectOfType<BlackHole_Grav>();

        if (blackHole != null)
        {
            radius = blackHole.Radius;
        }
        else
        {
            Debug.LogError("No Black Hole found in the scene.");
        }
        StartCoroutine(randomSpawner());
    }

    private void SpawnEnemy()
    {
        // Generate a random position on the outer edge of the radius and keep them on an even y plane for testing
        //the code for the edge spawner was found on the Unity discussions page, commented by Baste, which was found after prior testing with onUnitSphere.
        //source: https://discussions.unity.com/t/no-random-onunitcircle/587504 

        //
        //Vector2 edgeSpawner = Random.insideUnitCircle.normalized * radius;
        ////from here this is my own code
        //// Initialize array to store 4 spawn positions
        //Vector3[] spawnPositions = new Vector3[4];

        //// Calculate the four positions symmetrically
        //spawnPositions[0] = new Vector3(edgeSpawner.x, 0, edgeSpawner.y);         // Original position
        //spawnPositions[1] = new Vector3(-edgeSpawner.x, 0, edgeSpawner.y);        // Mirror on X-axis
        //spawnPositions[2] = new Vector3(-edgeSpawner.x, 0, -edgeSpawner.y);        // Mirror on Z-axis
        //spawnPositions[3] = new Vector3(edgeSpawner.x, 0, -edgeSpawner.y);       // Mirror on both axes

        // Generate a random angle to start spawning positions
        float startAngle = Random.Range(0f, 360f);

        // Initialize array to store 4 spawn positions
        Vector3[] spawnPositions = new Vector3[4];

        // Calculate the four positions symmetrically, 90 degrees apart
        for (int i = 0; i < 4; i++)
        {
            float angle = startAngle + i * 90f;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            spawnPositions[i] = new Vector3(x, 0, z);
        }

        // Randomly choose an enemy to spawn
        float randomEnemy = Random.Range(0f, 100f);
        int ChosenEnemy;

        if (randomEnemy < 80f)
        {
            ChosenEnemy = 0;
        }

        else
        {
            ChosenEnemy = 1;
        }


        // Instantiate enemies at the calculated positions
        foreach (Vector3 pos in spawnPositions)
        {

            Instantiate(EnemyPrefab[ChosenEnemy], pos, Quaternion.identity);
        }
    }
}
