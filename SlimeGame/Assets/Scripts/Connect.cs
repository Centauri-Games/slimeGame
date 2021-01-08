using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Connect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    public GameObject connecting;
    public GameObject buton1;
    public GameObject buton2;
    public GameObject buton3;

    public GameObject buton4;

    public void Connecttoserver()
    {
        buton1.SetActive(false);
        buton2.SetActive(false);
        buton3.SetActive(false);
        buton4.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                connecting.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("GameMenu");
            }
        }
    }
    public override void OnConnectedToMaster(){
        SceneManager.LoadScene("Menu");
        
    }
}
