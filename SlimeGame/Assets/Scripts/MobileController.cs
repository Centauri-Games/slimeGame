using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] FixedJoystick cameraJoystick;
    [SerializeField] LongClickButton shootButton;
    [SerializeField] GameObject mobileHUD;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject statsButton;
    bool isJumping = false;
    
    bool isSwitching = false;

    public bool startShoot = false;
    public bool endShoot = false;
    bool isShooting = false;

    public void Start()
    {
        Debug.Log("Es móvil?" + MobileChecker.isMobile());

        if (MobileChecker.isMobile())
        {
            mobileHUD.SetActive(true);
            pauseButton.SetActive(true);
            statsButton.SetActive(true);
        }
        else
        {
            mobileHUD.SetActive(false);
            pauseButton.SetActive(false);
            statsButton.SetActive(false);
        }

        shootButton.mc = this;
    }
    public void LateUpdate()
    {
        isJumping = false;
        isSwitching = false;
    }


    public Vector2 getDirection()
    {
        return joystick.Direction;
    }

    public Vector2 getCameraDirection()
    {
        return cameraJoystick.Direction;
    }

    public bool IsJumping()
    {
        return isJumping;
    }
    
    public bool StartShoot()
    {
        if (startShoot)
        {
            startShoot = false;
            isShooting = true;
            return true;
        }
        return false;
    }

    public bool EndShoot()
    {
        if (endShoot)
        {
            endShoot = false;
            isShooting = false;
            return true;
        }
        return false;
    }

    public bool IsSwitching()
    {
        return isSwitching;
    }

    public bool isRunning()
    {
        if (joystick.Horizontal > 0.6f || joystick.Vertical > 0.6f)
        {
            return true;
        }
        return false;
    }

    public void shoot()
    {
        isShooting = true;
    }

    public void jump()
    {
        isJumping = true;
    }

    public void switching()
    {
        isSwitching = true;
    }
}
