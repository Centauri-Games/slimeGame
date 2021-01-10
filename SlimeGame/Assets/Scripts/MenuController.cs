using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MenuController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public GameObject connecting;
    public GameObject nickname;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;

    public void Connecttoserver()
    {
        nickname.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                connecting.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    public override void OnConnectedToMaster(){
        SceneManager.LoadScene("GameMenu");
        
    }
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume",1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void showNickname(){
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
         nickname.SetActive(true);
    }
    public void goToMatchmakingScene(){
        SceneManager.LoadScene("GameMenu");
    }

    public void goBackToMenu(){
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
    
    public void goToCustomizationMenu()
    {
        SceneManager.LoadScene("Customization");
    }

    public void goToShopScene(){
        SceneManager.LoadScene("ShopScene");
    }

    
    
}
