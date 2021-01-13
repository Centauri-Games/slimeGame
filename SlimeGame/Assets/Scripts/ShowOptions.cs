using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ShowOptions : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    [SerializeField] GameObject options;
    [SerializeField] GameObject ingame;
    static bool isPaused = false;

    bool mobile = MobileChecker.isMobile();

    void Start()
    {
        isPaused = false;
        if (!mobile)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            showOptions();
        }
    }

    public void showOptions()
    {
        if (!options.active)
        {
            isPaused = true;
            if (!mobile)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            options.SetActive(true);
            ingame.SetActive(false);
        }
        else
        {
            isPaused = false;
            if (!mobile)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            options.SetActive(false);
            ingame.SetActive(true);
        }
    }

    public void goBackToMenu()
    {
        GameManager.am.playMusic(3);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("MainMenu");
    }

    public static bool IsPaused()
    {
        return isPaused;
    }
}
