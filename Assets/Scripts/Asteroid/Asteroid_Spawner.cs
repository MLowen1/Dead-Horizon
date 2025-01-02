using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Spawner : MonoBehaviour
{
    [Header("Asteroid prefab")]
    public GameObject[] asteroidPrefab;
    public bool IsSpawning = true;
    private float radius;
    [SerializeField] private int initialSpeed;

    [Header("Spawn time")]
    [SerializeField] private float minspawntime = 1;
    [SerializeField] private float maxspawntime = 5;

    [Header("target location")]
    [SerializeField][Range(1f, 5f)] float distFromCentre;
    private BlackHole_Grav blackHole;
    // Coroutine to spawn asteroids at random intervals
    private IEnumerator randomSpawner()
    {
        while (IsSpawning)
        {
            SpawnAsteroid();

            float spawnInterval = Random.Range(minspawntime, maxspawntime);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Start()
    {
        blackHole = FindObjectOfType<BlackHole_Grav>();

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

    private void SpawnAsteroid()
    {
        // Generate a random position on the outer edge of the radius and keep them on an even y plane for testing
        //the code for the edge spawner was found on the Unity discussions page, commented by Baste (2015), which was found after prior testing with onUnitSphere.
        //source: https://discussions.unity.com/t/no-random-onunitcircle/587504 
        Vector2 edgeSpawner = Random.insideUnitCircle.normalized * radius;
        //from here this is my own code
        Vector3 SpawnPosition;
        SpawnPosition.x = edgeSpawner.x;
        SpawnPosition.y = 2;
        SpawnPosition.z = edgeSpawner.y; //this sets the vector3 z value to the vector2 y value as vec2 is 2 dimensional therefore y is actually z

        int randomNum = Random.Range(0, asteroidPrefab.Length -1);

        GameObject newAsteroid = Instantiate(asteroidPrefab[randomNum], SpawnPosition, Quaternion.identity);
        Rigidbody rb = newAsteroid.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //reuse the previous circle calc to decide how the target initial tragectory of the asteriods will be decided
            float radiusMod = radius / distFromCentre;
            Vector2 TargetLoc = Random.insideUnitCircle.normalized * radiusMod;
            Vector3 TargetPos;
            TargetPos.x = TargetLoc.x;
            TargetPos.y = 2;
            TargetPos.z = TargetLoc.y; 

            Vector3 directionToTarget = (TargetPos - SpawnPosition).normalized;

            rb.velocity = directionToTarget * initialSpeed;
        }

    }

    //this activates a gizmo to display the radius- this code was taken from stack overflow and is simply for debugging purposes only.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}

