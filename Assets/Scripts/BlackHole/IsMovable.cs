using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class IsMovable : MonoBehaviour
{
    //this class will define the effects that the black hole will have on an object.
    // Minimum and maximum velocity for objects being pulled 
    [Header("Gravitational Velocity")]
    public float minVelocity = 1f;
    public float maxVelocity = 10f;
    public bool canMove = true;

}
