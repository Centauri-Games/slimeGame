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
    public GameObject button7;
    public GameObject button8;
    bool ready;
    public GameObject ContactMenu;

    public void Connecttoserver()
    {
        PhotonNetwork.NickName = nickname.GetComponentInChildren<InputField>().text;
        if (PhotonNetwork.NickName.Equals(""))
        {
            PhotonNetwork.NickName = "Player" + Random.Range(0, 9999).ToString();
        }
        Debug.Log(PhotonNetwork.NickName);
        nickname.SetActive(false);
        ready = true;

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
        }else{
            SceneManager.LoadScene("GameMenu");
        }
    }
    public override void OnConnectedToMaster(){
        if(ready){
            SceneManager.LoadScene("GameMenu");

        }
        
        
    }
    void Start()
    {
        ready = false;
        AudioListener.volume = PlayerPrefs.GetFloat("volume",1);
    }

    public void updateGameMenuLenguage(){

    }
    public void updateMainMenuLanguage()
    {
        PlayerPrefs.SetInt("language", PlayerPrefs.GetInt("language"));
        if (PlayerPrefs.GetInt("language") == 1)
        {
            button1.GetComponentInChildren<Text>().text = "Jugar";
            button2.GetComponentInChildren<Text>().text = "Personalización";
            button3.GetComponentInChildren<Text>().text = "Tienda";
            connecting.GetComponentInChildren<Text>().text = "Conectando a los servidores...";
            nickname.GetComponentInChildren<Text>().text = "Introduce tu nombre de jugador";
            nickname.GetComponentInChildren<InputField>().GetComponentInChildren<Text>().text = "Nombre de usuario";
            nickname.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "Nombre de usuario";
            button7.GetComponentInChildren<Text>().text = "Contactar";
            button8.GetComponentInChildren<Text>().text = "¿Cómo jugar?";
        }
        else
        {
            button1.GetComponentInChildren<Text>().text = "Play";
            button2.GetComponentInChildren<Text>().text = "Customization";
            button3.GetComponentInChildren<Text>().text = "Shop";
            connecting.GetComponentInChildren<Text>().text = "Connecting to servers...";
            nickname.GetComponentInChildren<Text>().text = "Write your nickname";
            nickname.GetComponentInChildren<InputField>().GetComponentInChildren<Text>().text = "Nickname";
            nickname.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "Nickname";
            button7.GetComponentInChildren<Text>().text = "Contact";
            button8.GetComponentInChildren<Text>().text = "How to play?";
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Actualizar idioma de los textos
        updateMainMenuLanguage();
    }


    public void showNickname(){
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button7.SetActive(false);
        button8.SetActive(false);
        nickname.SetActive(true);
    }
    public void hideNickname()
    {
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
        button7.SetActive(true);
        button8.SetActive(true);
        nickname.SetActive(false);
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
    public void showContact(){
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        button4.SetActive(false);
        button7.SetActive(false);
        button8.SetActive(false);
        ContactMenu.SetActive(true);
    }

    public void hideContact(){

        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        button4.SetActive(true);
        button7.SetActive(true);
        button8.SetActive(true);
        ContactMenu.SetActive(false);
    }
    
    public void goToWebsite(int website){
        switch(website){
            case 0:
            Application.OpenURL("https://centauri-games.github.io/");
                break;
            case 1:
            Application.OpenURL("https://www.youtube.com/channel/UCMPGHT6gt11RJQFEf30gTKg?view_as=subscriber");
                break;
            case 2:
                Application.OpenURL("https://twitter.com/GamesCentauri");
                break;
            case 3:
                Application.OpenURL("https://github.com/Centauri-Games");
                break;
        }
    }

    
    
}
