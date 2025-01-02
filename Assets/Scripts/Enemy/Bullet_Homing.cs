using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Homing : IsMovable
{
    [Header("Bullet Lifetime")]
    [SerializeField] private float lifeTime = 5f;
    public int BulletDamage = 5;

    [Header("homing Strength")]
    [SerializeField] private float HomingIntensity = 5f;
    private Transform Player;
    private Rigidbody rb;
    public bool IsTracking = true;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Player = GameObject.FindWithTag("Player").transform;
        
        if (IsTracking && Player != null)
        {
            Vector3 directionToPlayer = (Player.position - transform.position).normalized;
            Vector3 homingVelocity = directionToPlayer * HomingIntensity;

            rb.velocity = homingVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);

            if (collision.gameObject.CompareTag("Player"))
            {
                if (!GameManager.instance.ShieldActive)
                {
                    GameManager.instance.playerHealth -= BulletDamage;
                }
            }
        }
    }
}
