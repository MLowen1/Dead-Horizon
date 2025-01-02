using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IsMovable
{
    [Header("Bullet Lifetime")]
    [SerializeField] private float lifeTime = 5f;
    public int BulletDamage = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Debug.Log("Bullet hit something");

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
