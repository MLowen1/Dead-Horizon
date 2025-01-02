using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Spawn time")]
    [SerializeField] private int ShieldHealth = 100;
    private GameManager gm;

    private void Start()
    {
       gm = GameManager.instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject entity = collision.gameObject;
        //Debug.LogError("Collided with " + entity);
        if (entity.CompareTag("Asteroid"))
        {
            ShieldHealth -= entity.gameObject.GetComponent<Asteroid>().Damage;
            Destroy(entity);
        }
        else if (entity.CompareTag("Bullet"))
        {
            int Damage;
            if (entity.GetComponent<Bullet>() != null)
            {
                Damage = entity.GetComponent<Bullet>().BulletDamage;
            }
            else if (entity.GetComponent<Bullet_Homing>() != null)
            {
                Damage = entity.GetComponent<Bullet_Homing>().BulletDamage;
            }
            else
            {
               Damage= 0;
                Debug.LogError("damage val not found");
            }
            ShieldHealth -= Damage;
            Destroy(entity);
        }

        if (ShieldHealth <= 0)
        {
            Destroy(gameObject);
            gm.ShieldActive = false;
        }
    }
}
