using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text text;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("language") == 1)
        {
            text.text = "FIN DE LA PARTIDA";
            button.GetComponentInChildren<Text>().text = "Volver al menú";
        }
        else
        {
            text.text = "GAME OVER";
            button.GetComponentInChildren<Text>().text = "Return to main menu";
        }
    }

    public void goBackToMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
