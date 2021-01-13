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

    public Text[] panelTexts;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("language") == 1)
        {
            for (int i = 0; i < panelTexts.Length; i++)
            {
                panelTexts[i].text = "Esperando a otro jugador...";
            }
        }
        else
        {
            for (int i = 0; i < panelTexts.Length; i++)
            {
                panelTexts[i].text = "Waiting for other player...";
            }
        }
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
            button4.GetComponentInChildren<Text>().text = "Empezar";
        }
        else
        {
            button1.GetComponentInChildren<Text>().text = "1 VS. 1";
            button2.GetComponentInChildren<Text>().text = "2 VS. 2";
            button3.GetComponentInChildren<Text>().text = "Free for all";
            button4.GetComponentInChildren<Text>().text = "Start";
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
