using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : IsMovable
{
    // This is the playerInput component attached to the PlayerController gameObject.
    PlayerInput playerInput;
    // This acts as the binding that allows for the input action to be called for movement.
    InputAction moveAction;
    InputAction extractAction;

    // This sets a header above the playermodel variable in the inspection window.
    [Header("Player Appearance")]
    [SerializeField] public GameObject playerModel;

    [Header("Movement Variables")]
    //This declares the global variable of what the movement speed will (this can be change in the inspection window).
    [SerializeField] private float movementSpeed = 5f;
    // This declares an empty float variable that will take the place of velocity as i did not want this to effect the rotation.
    private float a = 5f;

    public bool Extracting;
    private Asteroid asteroid;
    public GameManager gm;

    // Flag to indicate if the player is colliding with a boundary
    private bool isCollidingWithBoundary = false;


    void Awake()
    {
        gm = GameManager.instance;
        if (gm == null)
        {
            Debug.LogError("GameManager instance is null. Ensure GameManager is correctly set up as a singleton.");
        }
        // The playerInput variable is defined by getting it from the player
        playerInput = GetComponent<PlayerInput>();
        // This uses the find action to locate the action responsible for movement in the action maps created.
        moveAction = playerInput.actions.FindAction("Movement Action (PC)");
        extractAction = playerInput.actions.FindAction("Extract (PC)");

    }

    // This method is responsbile for getting the players input and returning it.
    private Vector2 GetPlayerInputs()
    {
        // This returns the the value for inputs 
        if (playerInput == null) { Debug.Log("player input not found"); }
        return moveAction.ReadValue<Vector2>();
    }

    // This is a void type method responsible for the movement of the player. 
    private void MovePlayer(Vector2 _direction)
    {
        // This checks to see that the direction being used is not a null value.
        if (_direction != null && gm.resources > 0 && !isCollidingWithBoundary)
        {
            // The players input is a Vector2 but the level is in a 3D space so a Vector 3 is intitalised. It uses the x axis of the direction for the x of the Vector3.
            // The Y of the Vector3 is set to zero. Then the y axis of the direction is used as the z for the Vector3.
            //Finally this is times by the players movement speed and time.delta time so the transform doesnt occur instantainously.
            transform.position += new Vector3(_direction.x, 0, _direction.y) * movementSpeed * Time.deltaTime;
        }

    }

    // This is a void type method responsible for the rotation of the player.
    private void RotatePlayer(Vector2 _direction)
    {
        // This checks to see that the direction is not a null value
        if (_direction != null)
        {
            // This checks against a Vector2 of (0,0) to make sure that the direction is not equal to no input being provided.
            if (_direction != Vector2.zero)
            {
                // This is the declaration of the angle variable using the Mathf.SmoothDampAngle Method. The first parameter takes the y of the model for the ship using Transform.eulerAngles (This returns the angle of the transform in degrees.
                // The third parameter is used to reference a velocity however, as i did not want this to effect the rotation i used the aribtary value a declared at the start.
                // The last parameter is responsible for the time for the angle to smooth.
                float angle = Mathf.SmoothDampAngle(playerModel.transform.eulerAngles.y, Mathf.Rad2Deg * Mathf.Atan2(_direction.x, _direction.y) + 180, ref a, 0.15f);
                // This uses Quaternion.Euler() which takes in x,y,z floats and sets the rotation based on those values. As rotation is around the Y axis only this variable is set as the previous angle variable.
                playerModel.transform.rotation = Quaternion.Euler(0, angle, 0);
            }

        }
    }

    // This has been changed to fixedUpdate so that the code is executed at a predefined speed rather tham the usual every frame per second.
    void FixedUpdate()
    {

        // This calls the MovePlayer method, as it has a parameter of a Vector2 GetPlayerInputs is used in accordance. This is done as GetPlayerInputs returns a vector2.
        MovePlayer(GetPlayerInputs());
        // This calls the RotatePlayer method, as it has a parameter of a Vector2 GetPlayerInputs is used in accordance. This is done as GetPlayerInputs returns a Vector2.
        RotatePlayer(GetPlayerInputs());

        if (moveAction.IsPressed())
        {
            gm.ismoving = true;
        }
        else
        {
            gm.ismoving = false;
        }
    }

    // This method is called when the player collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Boundary"
        if (collision.gameObject.CompareTag("Boundary"))
        {
            isCollidingWithBoundary = true;
        }
    }

        // This method is called when the player stops colliding with another object
    private void OnCollisionExit(Collision collision)
    {
        // Check if the collided object has the tag "Boundary"
        if (collision.gameObject.CompareTag("Boundary"))
        {
            isCollidingWithBoundary = false;
        }
    }
}
