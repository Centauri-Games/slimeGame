using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class PlayerController : MonoBehaviour
{

    //Photon
    PhotonView id;

    public Transform cam;

    //Movimiento
    [SerializeField] float regularSpeed = 2.0f;
    [SerializeField] float runSpeed = 3.0f;

    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float doubleJumpMult = 0.75f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] bool doubleJump = false;
    [SerializeField] float bounceMultiplier = 3.0f;

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

    //Trepar
    bool canClimb = false;
    float climbStamina = 5;
    float maxClimbSt = 5;

    Rect staminaBar;
    Texture2D staminaTex;

    public void Awake()
    {

        characterController = GetComponent<CharacterController>();
        climbStamina = maxClimbSt;

        id = GetComponent<PhotonView>();

        if (id.IsMine)  //Para evitar multiples instancias de la barra de stamina
        {
            staminaBar = new Rect(Screen.width / 10, Screen.height * 9 / 10, Screen.width / 3, Screen.height / 50);
            staminaTex = new Texture2D(1, 1);
            staminaTex.SetPixel(0, 0, Color.blue);
            staminaTex.Apply();
        }

    }

    void Start()
    {

        if (!id.IsMine)
        {
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
            Destroy(GetComponentInChildren<Rigidbody>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!id.IsMine) return;

        handleMove();
        handleJump();
        handleCamera();
    }

    public void handleMove()
    {
        if (canClimb && !characterController.isGrounded)    //Si está trepando
        {
            if (climbStamina > 0)
            {
                moveInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
                climbStamina -= Time.deltaTime;
                if (climbStamina < 0) climbStamina = 0;
            }
        }
        else    //Si no puede trepar
        {
            float y = moveInput.y;
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
            moveInput = transform.TransformDirection(moveInput);

            if (Input.GetButton("Run"))
            {
                moveInput *= runSpeed;
            }
            else
            {
                moveInput *= regularSpeed;
            }

            moveInput.y = y;

            if (climbStamina < maxClimbSt)
            {
                climbStamina += Time.deltaTime;
                if (climbStamina > maxClimbSt) climbStamina = maxClimbSt;
            }
        }
        characterController.Move(moveInput * Time.deltaTime);

    }

    public void handleJump()
    {

        if (characterController.isGrounded || canClimb) //Si está trepando o en el suelo
        {
            doubleJump = true;  //Se reactiva el doble salto


            if (moveInput.y <= 0 && characterController.isGrounded)   //Solo cuando está en el suelo
            {
                moveInput.y = 0;
            }


            if (Input.GetButtonDown("Jump"))    //Salto si suelo o pared
            {
                if (!canClimb)
                    moveInput.y = jumpHeight;
                else
                {
                    moveInput.y = jumpHeight;
                    canClimb = false;   //Realiza el salto y desactiva la escalada
                }
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

    public void enableClimb()
    {
        canClimb = true;
    }

    public void disableClimb()
    {
        canClimb = false;
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Wall"))
        {
            enableClimb();
        }
        if(go.CompareTag("Bounce")) //Rebote
        {
            moveInput.y = jumpHeight * bounceMultiplier;
            characterController.Move(moveInput*Time.deltaTime);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Wall"))
        {
            disableClimb();
        }
    }

    void OnGUI()
    {
        if (id.IsMine)
        {
            float ratio = climbStamina / maxClimbSt;
            float barWidth = ratio * Screen.width / 3;
            staminaBar.width = barWidth;
            GUI.DrawTexture(staminaBar, staminaTex);
        }
    }

}


