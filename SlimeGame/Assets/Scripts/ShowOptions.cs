using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShowOptions : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    [SerializeField] GameObject options;
    [SerializeField] GameObject ingame;

    void Start()
    {
        

        
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
            options.SetActive(true);
            ingame.SetActive(false);
        }
        else
        {
            options.SetActive(false);
            ingame.SetActive(true);
        }
    }

    public void goBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
