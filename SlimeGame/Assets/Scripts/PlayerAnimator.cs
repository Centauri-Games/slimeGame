using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator slimeAnimatorController;

    CharacterController characterController;

    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        slimeAnimatorController.SetBool("Grounded", true);
    }

    // Update is called once per frame
    void Update()
    {
        slimeAnimatorController.SetBool("Moving", false);

        if (Input.GetButtonDown("Run"))
        {
            slimeAnimatorController.SetBool("Moving", true);
        }

        if (Input.GetButtonDown("Jump"))    //Salto si suelo o pared
        {
            slimeAnimatorController.SetBool("Grounded", false);
        }
        
        if (characterController.isGrounded)
        {
            slimeAnimatorController.SetBool("Grounded", true);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Wall"))
        {
            slimeAnimatorController.SetBool("WallPasted", true);
        }
        else if (go.CompareTag("Bounce")) //Rebote
        {
            slimeAnimatorController.SetBool("Grounded", false);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.CompareTag("Wall"))
        {
            slimeAnimatorController.SetBool("WallPasted", false);
        }
    }
}
