using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Speed of rotation in degrees per second
    [SerializeField] private float rotationSpeed = 90f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Y-axis at the specified speed
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}