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
        PlayerPrefs.SetInt("language", 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Actualizar idioma de los textos
        PlayerPrefs.SetInt("language", PlayerPrefs.GetInt("language"));
        if (PlayerPrefs.GetInt("language") == 1)
        {
            button1.GetComponentInChildren<Text>().text = "Jugar";
            button2.GetComponentInChildren<Text>().text = "Personalización";
            button3.GetComponentInChildren<Text>().text = "Tienda";
            button4.GetComponentInChildren<Text>().text = "Opciones";
            connecting.GetComponentInChildren<Text>().text = "Conectando a los servidores...";
            nickname.GetComponentInChildren<Text>().text = "Introduce tu nombre de jugador";
            nickname.GetComponentInChildren<InputField>().GetComponentInChildren<Text>().text = "Nombre de usuario";
            nickname.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "Nombre de usuario";
        }
        else
        {
            button1.GetComponentInChildren<Text>().text = "Play";
            button2.GetComponentInChildren<Text>().text = "Customization";
            button3.GetComponentInChildren<Text>().text = "Shop";
            button4.GetComponentInChildren<Text>().text = "Options";
            connecting.GetComponentInChildren<Text>().text = "Connecting to servers...";
            nickname.GetComponentInChildren<Text>().text = "Write your nickname";
            nickname.GetComponentInChildren<InputField>().GetComponentInChildren<Text>().text = "Nickname";
            nickname.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "Nickname";
        }
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
