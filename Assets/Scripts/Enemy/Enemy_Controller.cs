using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    //components necessary for world tracking
    [Header("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform blackHoleTransform;
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Transform[] weaponSources;
    [SerializeField] private float enemyLifespan = 30f;

    //movement variables
    [Header("Movement")]   
    [SerializeField] private float Speed = 1f;
    [SerializeField] private float Radius_offset = 0f;
    [SerializeField] private bool lookAtPlayer = false;
    private float orbit_Radius;
    private float rotation = 0f;

    private Vector3 initialOffset;
    private float initialAngle;
    private float initialDistance;

    //bullet variables
    [Header("Bullets")]
    [SerializeField] private float FireDelay = 2f; 
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private bool CanAttack = true;
    [SerializeField] private AudioClip BulletSoundClip;

    //score variable
    [Header("Score")]
    [SerializeField] private int scoreValue = 100;

    private int ChosenBullet;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        GameObject playerTarget = GameObject.FindWithTag("Player");
        GameObject blackHoleTarget = GameObject.FindWithTag("BlackHole");

        playerTransform = playerTarget.transform;
        blackHoleTransform = blackHoleTarget.transform;

        ChosenBullet = 0;

        // Old code for chosing between the random bullets
        //float randomValue = Random.Range(0f, 100f);

        //if (randomValue < 90f)
        //{
        //    ChosenBullet = 0;
        //}
        //else
        //{
        //    ChosenBullet = 1;
        //}

        BlackHole_Grav blackHole = FindObjectOfType<BlackHole_Grav>();

        Vector3 blackHolePosition = blackHole.transform.position;
        initialOffset = transform.position - blackHolePosition; 
        initialAngle = Mathf.Atan2(initialOffset.z, initialOffset.x);

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Attack());
        StartCoroutine(AttackCooldown());
        StartCoroutine(SwitchDir());

        Destroy(gameObject, enemyLifespan);
        Debug.Log("Start escaped");
    }

    void Update()
    {
        Move();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (CanAttack)
            {
                yield return new WaitForSeconds(FireDelay);

                if (playerTransform != null)
                {
                    DistanceToPlayer();
                    ShootAtTarget();
                    audioSource.clip = BulletSoundClip;
                    audioSource.Play();
                }
            }

            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            CanAttack = false;
            Debug.Log("Can attack again");

            yield return new WaitForSeconds(5f);
            CanAttack = true;
            Debug.Log("Can't attack");
        }
    }

    private void DistanceToPlayer()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Calculate the distance to the player
        initialDistance = Vector3.Distance(playerTransform.position, transform.position);

        //Debug.Log("Distance to player: " + initialDistance);

        if (initialDistance < 15)
        {
            ChosenBullet = 1;
            lookAtPlayer = true;
        }

        else
        {
            ChosenBullet = 0;
            lookAtPlayer = false;
        }
    }

    private void ShootAtTarget()
    {
        for (int i = 0; i < weaponSources.Length; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab[ChosenBullet], weaponSources[i].position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                //set velocity towards the black hole
                Vector3 directionToBlackHole = (blackHoleTransform.position - transform.position).normalized;
                rb.velocity = directionToBlackHole * bulletSpeed;
            }
        }
    }

    private void Move()
    {
        OrbitBlackHole();
        if (lookAtPlayer)
        {
            LookAtPlayer();
        }

        else
        {
            LookAtBlackHole();
        }
    }

    private void OrbitBlackHole()
    {
        // find the black hole in scene and set it to the radius of the black hole with a definable radius offset to have them spawn closer or further away.
        GameObject BlackHole = GameObject.FindGameObjectWithTag("BlackHole");
        if (BlackHole != null)
        {
            
            orbit_Radius = BlackHole.GetComponent<BlackHole_Grav>().Radius - Radius_offset;
            

        }

        // Calculate the new rotation in the circular orbit based on the speed of the object and time past, to create a gradua, slower rotation
        rotation += Speed * Time.deltaTime;

        //ensure that the rotator resets after 360 degrees of rotation
        float currentAngle = initialAngle + rotation;
        //calculate and setthe new position based on the oscillation of cos and sine waves (did have to look up the calculation) against a radius
        //with inspiration from Dhi Games video on how to instantiate objects in a circle formation (2021): https://www.youtube.com/watch?v=V68OVgywm2A
        Vector3 NewPos = new Vector3(Mathf.Cos(currentAngle) * orbit_Radius, 2f, Mathf.Sin(currentAngle) * orbit_Radius) ;

        transform.position = BlackHole.transform.position + NewPos;
    }

    private void LookAtPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 PlayerDierction = playerTransform.position - transform.position;
            Quaternion LookAt = Quaternion.LookRotation(PlayerDierction);
            transform.rotation = LookAt;
        }

        else
        {
            Debug.LogError("Player reference is missing!");
        }
    }

    private void LookAtBlackHole()
    {
        if (blackHoleTransform != null)
        {
            Vector3 blackHoleDirection = blackHoleTransform.position - transform.position;
            Quaternion LookAt = Quaternion.LookRotation(blackHoleDirection);
            transform.rotation = LookAt;
        }

        else
        {
            Debug.LogError("Player reference is missing!");
        }
    }

    private IEnumerator SwitchDir()
    {
        while(true) {
            if (Speed < 10 && Speed > -10)
            {
                Speed *= 1.2f;
            }
            Speed = -Speed;
            //Debug.Log("change dir");
            yield return new WaitForSeconds(10);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            //find asteriod script
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null && asteroid.ThrownByPlayer)
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
                // Add score if asteroid was thrown by the player here
                GameManager.instance.score += scoreValue;
            }
        }
    }
}
