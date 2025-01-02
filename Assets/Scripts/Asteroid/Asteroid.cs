using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : IsMovable
{
    //here is the base values for the asteroids
    [Header("Statistics")]
    [SerializeField] private float ResourceVal = 10;
    public int Damage = 20;
    [SerializeField] private int lifetime = 20;

    public bool ThrownByPlayer = false;

    private GameManager gm;
    public bool IsExtracting = false;
    
    // Coroutine for shooting at the player
    private IEnumerator gradualCollection()
    {
        IsExtracting = true;
        while (GetComponent<Interaction>().Extracting)
        {
            yield return new WaitForSeconds(gm.collectionSpeed);

            if (ResourceVal > 0)
            {
                ResourceVal -= gm.collectionRate;
                if (gm.resources < gm.MaxResource)
                {
                    gm.resources += gm.collectionRate;
                }
                
                gm.IsSkillActive();
            }

            else
            {
                Debug.Log("Asteroid extracted");
                GetComponent<Interaction>().Extracting = false;
                IsExtracting = false;
                DestroyAsteroid();
                yield break;
            }
        }
    }

    private void Start()
    {
        gm = GameManager.instance;
        Destroy(gameObject, lifetime);
    }

    public void CollectAsteroid()
    {
        if (!IsExtracting)
        {
            Debug.Log("Is extracting");
            StartCoroutine(gradualCollection());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject entity = collision.gameObject;

        //if (entity.CompareTag("Enemy"))
        //{
        //   Destroy(entity);
        //   DestroyAsteroid();
        //}

        if (entity.CompareTag("Player"))
        {
            if (!gm.ShieldActive)
            {
                gm.playerHealth -= Damage;
                DestroyAsteroid();
            } 
        }
    }

    public void DestroyAsteroid()
    {
        //Debug.Log("asteroid destroyed");
        Destroy(gameObject);
    }
}
