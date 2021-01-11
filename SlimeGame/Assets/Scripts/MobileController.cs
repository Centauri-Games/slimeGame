using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick;
    bool isJumping = false;
    bool isShooting = false;
    bool isSwitching = false;
    // Start is called before the first frame update

    public void Update()
    {
        Debug.Log("Vector: " + joystick.Direction);
    }


    public Vector2 getDirection()
    {
        return joystick.Direction;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
    public bool IsShooting()
    {
        return isShooting;
    }

    public bool IsSwitching()
    {
        return isSwitching;
    }
}
