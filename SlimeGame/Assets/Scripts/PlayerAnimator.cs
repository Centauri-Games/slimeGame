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
        
    }

    // Update is called once per frame
    void Update()
    {
        slimeAnimatorController.SetBool("Moving", false);

        if (Input.GetButton("Run"))
        {
            slimeAnimatorController.SetBool("Moving", true);
        }

        if (characterController.isGrounded)
        {
            slimeAnimatorController.SetBool("Grounded", true);

            if (Input.GetButtonDown("Jump"))    //Salto si suelo o pared
            {
                slimeAnimatorController.SetBool("Grounded", false);
            }
        }


    }
}
