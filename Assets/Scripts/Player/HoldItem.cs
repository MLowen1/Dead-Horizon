using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldItem : MonoBehaviour
{
    //PlayerInput playerInput;
    //InputAction InteractAction;
    //InputAction ThrowAction;

    public GameObject ObjectBeingHeld;
    public Transform InteractionPoint;
    public Transform Center;
    public GameObject Player;
    PlayerMovement playerMovement;
    

    public float InteractionDistance;
    public float forceMultiplier;

    public bool CanThrow;
    public bool AsteroidIsInteracted;

    private Rigidbody rb;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //playerInput = GetComponent<PlayerInput>();
        //InteractAction = playerInput.actions.FindAction("Hold (PC)");
        //ThrowAction = playerInput.actions.FindAction("Throw (PC)");

        rb = GetComponent<Rigidbody>();
        Center = GameObject.Find("Center").transform;
        InteractionPoint = GameObject.Find("InteractionPoint").transform;
    }

    //private void GetPlayerInputs()
    //{

    //}

    //private void InteractAsteroid()
    //{

    //}

    //private void ThrowAsteroid()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //InteractAsteroid(GetPlayerInputs());
        //ThrowAsteroid(GetPlayerInputs());

        if (playerMovement != null)
        {
            Center.rotation = playerMovement.playerModel.transform.rotation;
        }

        if (Input.GetKey(KeyCode.E) && AsteroidIsInteracted == true && CanThrow)
        {
            forceMultiplier += 300 * Time.deltaTime;
        }

        InteractionDistance = Vector3.Distance(Center.position, transform.position);

        if (InteractionDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && AsteroidIsInteracted == false)
            {
                ObjectBeingHeld.transform.position = InteractionPoint.transform.position;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<SphereCollider>().enabled = false;
                //this.transform.position = InteractionPoint.position;
                //this.transform.parent = GameObject.Find("InteractionPoint").transform;

                AsteroidIsInteracted = true;
                forceMultiplier = 0;
            }
        }

        if(Input.GetKeyUp(KeyCode.E) && AsteroidIsInteracted == true)
        {
            CanThrow = true;

            if (forceMultiplier > 10)
            {
                rb.AddForce(Player.transform.forward * forceMultiplier);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<SphereCollider>().enabled = true;
                AsteroidIsInteracted = false;

                forceMultiplier = 0;
                CanThrow = false;
            }

            forceMultiplier = 0;
        }
    }    
}
