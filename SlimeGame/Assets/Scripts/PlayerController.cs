using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Movimiento
    public float regularSpeed = 2.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    Vector3 moveInput = Vector3.zero;   //Vector de movimiento


    //Camara
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public float minimumX = -360f;
    public float maximumX = 360f;
    public float minimumY = -60f;
    public float maximumY = 60f;

    float rotationY = 0f;

    
    CharacterController characterController;

    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        handleMove();
        handleCamera();
    }

    public void handleMove()
    {

        bool groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && moveInput.y < 0)
        {
            moveInput.y = 0f;
        }

        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));    //Recoge valores vertical y horizontal de los controles
        moveInput = transform.TransformDirection(moveInput);
        moveInput.y = 0.0f;
        moveInput *= regularSpeed;

        if (characterController.isGrounded && Input.GetButtonDown("Jump")) //Si está en el suelo, puede saltar
        {
            moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        moveInput.y += gravity * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);

        
        // Salto
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            moveInput.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        moveInput.y += gravity * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
    }

    public void handleCamera()
    {

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

    }

    public float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}


