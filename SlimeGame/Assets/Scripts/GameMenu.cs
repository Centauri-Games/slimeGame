using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameMenu : MonoBehaviourPunCallbacks
{
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("language", PlayerPrefs.GetInt("language"));
        if (PlayerPrefs.GetInt("language") == 1)
        {
            button1.GetComponentInChildren<Text>().text = "1 contra 1";
            button2.GetComponentInChildren<Text>().text = "2 contra 2";
            button3.GetComponentInChildren<Text>().text = "Todos contra todos";
            button4.GetComponentInChildren<Text>().text = "Atrás";
        }
        else
        {
            button1.GetComponentInChildren<Text>().text = "1 VS. 1";
            button2.GetComponentInChildren<Text>().text = "2 VS. 2";
            button3.GetComponentInChildren<Text>().text = "All VS. All";
            button4.GetComponentInChildren<Text>().text = "Back";
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
