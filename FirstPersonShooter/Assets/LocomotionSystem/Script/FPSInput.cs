using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // enforces dependency on character controller
public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpSpeed = 15.0f;
    public float terminalVelocity = -20.0f;
    private float vertSpeed;

    // reference to the character controller
    private CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        // get the character controller component
        charController = GetComponent<CharacterController>();
        vertSpeed = 0;
     
    }

    private void ControlMovement()
    {
        Vector3 movement = Vector3.zero;

        // Handle WASD input
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        if (deltaX != 0 || deltaZ != 0)
        {
            movement = new Vector3(deltaX, 0, deltaZ);

            // make diagonal movement consistent
            movement = Vector3.ClampMagnitude(movement, speed);

            // transform from local space to global space
            movement = transform.TransformDirection(movement);
        }


        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }
        else if (!charController.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {

                vertSpeed = terminalVelocity;
            }
        }

        // Apply vertical speed to movement
        movement.y = vertSpeed;

        // ensure movement is independent of the framerate
        movement *= Time.deltaTime;

        // Move the character controller
        charController.Move(movement);
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
    }
}
