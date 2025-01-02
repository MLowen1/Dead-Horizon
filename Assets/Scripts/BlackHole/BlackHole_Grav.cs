using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


public class BlackHole_Grav : MonoBehaviour
{
    // Gravitational pull strength
    [Header("Force")]
    [SerializeField] private float Grav_Force = 10f;
    // The radius in which objects are pulled towards the black hole and how strong the pull increases over distance

    [Header("Gravity radius")]
    public float Radius = 10f;
    [SerializeField] [Range(.1f,2f)]  float radius_EXPONENT = 2f;

    void FixedUpdate()
    {
        // in this section we find all entites within a given radius of the black hole and iterate through them.
        Collider[] entities = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in entities)
        {
            IsMovable movable = collider.GetComponent<IsMovable>();
            if (movable != null && movable.canMove)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // in this section the distance between the object and the black hole is calculated and then used to apply an exponential force based on distance and max velocities
                    Vector3 blackHoleDIR = (transform.position - rb.position).normalized;
                    float distanceToBlackHole = Vector3.Distance(transform.position, rb.position);
                    float pullMag = Grav_Force / Mathf.Pow(distanceToBlackHole, radius_EXPONENT);
                    rb.AddForce(blackHoleDIR * pullMag, ForceMode.Acceleration);

                    // this section adjusts the maximum velocity based on distance through linear interpolation. As the object gets closer to the black hole, max velocity increases, As the object gets farther away, max velocity decreases
                    float velOverDistance = Mathf.Lerp(movable.minVelocity, movable.maxVelocity, 1f - (distanceToBlackHole / Radius));

                    // set the object's velocity to the dynamically changing max velocity if it surpasses max velocity
                    if (rb.velocity.magnitude > velOverDistance)
                    {
                        rb.velocity = rb.velocity.normalized * velOverDistance;
                    }
                    //here we run a check to see if the object has passed the point of no return, at which point the object is destroyed (old, now done through collision)
                    /*if (distanceToBlackHole < 1f)
                    {
                        Destroy(collider.gameObject);
                    } */
                }
            }
        }
    }

    // collision script to destroy the enemy, checks if the asteroid was thrown by the player here to award points.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.playerHealth = 0;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.playerHealth = 0;
        }
        else if (collision.gameObject.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    //this activates a gizmo to display the radius- this code was taken from stack overflow and is simply for debugging purposes only.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}

