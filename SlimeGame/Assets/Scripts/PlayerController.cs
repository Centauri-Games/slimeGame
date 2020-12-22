using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform cam;

    //Movimiento
    [SerializeField] public float regularSpeed = 2.0f;
    [SerializeField]public float jumpHeight = 3.5f;
    [SerializeField]public float doubleJumpMult = 0.75f;
    [SerializeField]public float gravity = -9.81f;

    Vector3 moveInput = Vector3.zero;   //Vector de movimiento

    bool doubleJump = false;

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

    public void Start()
    {
        Debug.Log(jumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        handleMove();
        handleJump();
        handleCamera();
    }

    public void handleMove()
    {

        float y = moveInput.y;
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));    //Recoge valores vertical y horizontal de los controles
        moveInput = transform.TransformDirection(moveInput);
        moveInput *= regularSpeed;
        moveInput.y = y;

        characterController.Move(moveInput * Time.deltaTime);

    }

    public void handleJump()
    {
        if (characterController.isGrounded)
        {
            doubleJump = true;
            if (moveInput.y <= 0)
            {
                moveInput.y = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = jumpHeight;
            }
        }
        else
        {
            if (doubleJump && Input.GetButtonDown("Jump"))
            {
                moveInput.y = jumpHeight * doubleJumpMult;
                doubleJump = false;
            }
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
}


