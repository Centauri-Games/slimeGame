using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{

    //Photon
    PhotonView id;
    PlayerManager pm;

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

    Transform cameraHolder;

    float rotationY = 0f;


    CharacterController characterController;

    //Trepar
    bool canClimb = false;
    float climbStamina = 5;
    float maxClimbSt = 5;

    Rect staminaBar;
    Texture2D staminaTex;

    //Weapons
    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;

    //HP
    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    public void Awake()
    {

        characterController = GetComponent<CharacterController>();
        climbStamina = maxClimbSt;
        cameraHolder = transform.GetChild(0);

        id = GetComponent<PhotonView>();
        pm = PhotonView.Find((int)id.InstantiationData[0]).GetComponent<PlayerManager>();   //Busca el playerManager de la escena, dado su PhotonID

        characterController.detectCollisions = false;   //Ya lo detecta el collider propio
        ChangeItem(0);  //Activa la pistola de agua

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

        if (id.IsMine)
        {
            ChangeItem(0);  //Inicia con la pistola de agua
        }
        else    //Si no es el jugador local: destruye el rb y el manejador de cinemachine
        {
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!id.IsMine) return;

        handleMove();
        handleJump();
        handleCamera();
        handleWeaponChange();
        handleShoot();
        handleLimits();
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


        items[previousItemIndex].itemGameObject.transform.localEulerAngles = new Vector3(-rotationY*0.7f, 0, 0);   //Se aplica tambien la rotacion al arma actual

        cameraHolder.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);   //Se rota el cameraTarget para la vision superior e inferior
        transform.localEulerAngles = new Vector3(0, rotationX, 0);   //El jugador rota para la vision lateral

    }

    void handleShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (itemIndex == 0)
            {
                id.RPC("RPC_Shoot", RpcTarget.All);   //Lo lanza a todas las instancias de este jugador
            }
            else
            {
                RPC_Shoot();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (itemIndex == 0) //Si es la pistola de agua
            {
                id.RPC("RPC_End", RpcTarget.All);   //Lo lanza a todas las instancias de este jugador
            }
            else
            {
                RPC_End();
            }
        }
    }

    [PunRPC]
    void RPC_Shoot()
    {
        items[itemIndex].Use();
    }

    [PunRPC]
    void RPC_End()
    {
        items[itemIndex].End();
    }



    void handleLimits()
    {
        if(transform.position.y < -20f)
        {
            Die();
        }
    }

    void handleWeaponChange()
    {
        for(int i=0; i< items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                ChangeItem(i);
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(itemIndex >= items.Length - 1)
            {
                ChangeItem(0);
            }
            else
            {
                ChangeItem(itemIndex + 1);
            }
            
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                ChangeItem(items.Length -1);
            }
            else
            {
                ChangeItem(itemIndex -1);
            }
        }
    }
    void ChangeItem(int itemId)
    {
        if (itemId == previousItemIndex) return;

        itemIndex = itemId;
        items[itemIndex].itemGameObject.SetActive(true);

        if(itemIndex == 0)
        {
            ((WaterGun)items[itemIndex]).Stop();
        }

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (id.IsMine)      //Sincronizacion del arma actual
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!id.IsMine && targetPlayer == id.Owner)   //Si es el jugador que ha cambiado el arma y no es el local
        {
            ChangeItem((int)changedProps["itemIndex"]);
        }
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
        else if(go.CompareTag("Bounce")) //Rebote
        {
            moveInput.y = jumpHeight * bounceMultiplier;
            characterController.Move(moveInput*Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (go.GetComponent<AmmoRecharge>() != null)
        {
            ((WaterGun)items[0]).Recharge();    //Si es un objeto de munición
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

    public void TakeDamage(float dmg)
    {
        Debug.Log("Me hizo pupita");
        id.RPC("RPC_TakeDamage", RpcTarget.All, dmg);   //Lo lanza a todas las instancias de este jugador
    }

    [PunRPC]
    void RPC_TakeDamage(float dmg)
    {
        if (!id.IsMine) return; //Solo se ejecuta en el ordenador del jugador alcanzado

        currentHealth -= dmg;
        Debug.Log("Took Damage: " + dmg);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        pm.Die();
    }
}


