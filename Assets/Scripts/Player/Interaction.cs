using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    private Transform InteractionPoint;
    private GameObject Player;
    private Transform PlayerModel;
    
    public float InteractionDistance;
    public float forceMultiplier;

    public bool CanThrow;
    public bool AsteroidIsInteracted;


    private Rigidbody rb;
    private AudioSource audioSource;

    [SerializeField] private AudioClip ExtractSoundClip;

    PlayerInput HoldInput;
    InputAction HoldAction;
    InputAction ThrowAction;
    InputAction DropAction;
    InputAction extractAction;
    InputAction ExitAction;

    PlayerMovement playerMovement;

    public bool Extracting = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Finding the transform component of the PlayerController object and assigning it to Player
        Player = GameObject.Find("PlayerController 1");
        // Finding the transform component of the SM_Veh_Drone_Repair and assigning it to PlayerModel
        PlayerModel = GameObject.Find("SM_Veh_Drone_Repair_01").transform;
        // Finding the transform component of the InteractionPoint and assigning it to InteractionPoint
        InteractionPoint = GameObject.Find("InteractionPoint").transform;

        // The HoldInput variable is defined by getting the input from the player
        HoldInput = Player.GetComponent<PlayerInput>();
        // This uses the find action to locate the action responsible for holding an object in the created action map
        HoldAction = HoldInput.actions.FindAction("Hold (PC)");
        // This uses the find action to locate the action responsible for throwing an object in the created action map
        ThrowAction = HoldInput.actions.FindAction("Throw (PC)");
       // This uses the find action to locate the action responsible for dropping an object in the created action map
        DropAction = HoldInput.actions.FindAction("Drop (PC)");
        
        extractAction = HoldInput.actions.FindAction("Extract (PC)");

        ExitAction = HoldInput.actions.FindAction("Exit (PC)");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        // This checks to see if the hold action has been pressed (space bar) and that the boolean values are both set to true meaning the asteroid is being interacted with and is throwable
        if (HoldAction.IsPressed() && AsteroidIsInteracted == true && CanThrow)
        {
            GetComponent<Asteroid>().ThrownByPlayer = true;
            // This sets a base for the forcemultiplier variable 
            forceMultiplier = 50;
        }
        
        // This figures out the distance between the player and the object wanting to be picked up by the player and sets it to the InteractionDistance variable
        InteractionDistance = Vector3.Distance(Player.transform.position, transform.position);

        // This checks to see if the interaction distance is more than or equal to a given value
        if (InteractionDistance <= 6)
        {
            // If this is the case it next checks that the hold action has been pressed (space bar), and that the asteroid isnt already held. It also makes sure that the interactionPoint has less than one child object. This is done to stop from multiple asteroids being picked up at once.
            if (HoldAction.IsPressed() && AsteroidIsInteracted == false && InteractionPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<IsMovable>().canMove = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<SphereCollider>().enabled = true;
                this.transform.position = InteractionPoint.position;
                this.transform.parent = GameObject.Find("InteractionPoint").transform;

                AsteroidIsInteracted = true;
                CanThrow = true;
                forceMultiplier = 0;

            }
        }

        if (ThrowAction.IsPressed() && AsteroidIsInteracted == true && CanThrow)
        {
            CanThrow = true;
            if (forceMultiplier == 50)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                forceMultiplier = 1000;
                rb.AddForce(- PlayerModel.transform.forward * forceMultiplier, ForceMode.Impulse);
                this.transform.parent = null;
                GetComponent<IsMovable>().canMove = true;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<SphereCollider>().enabled = true;
                AsteroidIsInteracted = false;

                forceMultiplier = 0;
                CanThrow = false;
                
            }

            forceMultiplier = 0;
        }

        if(DropAction.IsPressed() && AsteroidIsInteracted == true && CanThrow)
        {
            CanThrow = true;
            if (forceMultiplier == 50)
            {
                forceMultiplier = 10;
                rb.AddForce(PlayerModel.transform.forward * forceMultiplier );
                this.transform.parent = null;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent <IsMovable>().canMove = true;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<SphereCollider>().enabled = true;
                AsteroidIsInteracted = false;

                forceMultiplier = 0;
                CanThrow = false;
            }

            forceMultiplier = 0;
        }

        if (extractAction != null)
        {
           
            if (extractAction.IsPressed() && AsteroidIsInteracted == true && Extracting == false)
            {

                Extracting = true;
                GetComponent<Asteroid>().CollectAsteroid();
                //GetComponent<Asteroid>().IsExtracting = true;
                audioSource.clip = ExtractSoundClip;
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("Does not exsist");
        }


        if (ExitAction.IsPressed())
        {
            Application.Quit();
        }
    }
}
