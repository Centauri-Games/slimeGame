using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameOver : MonoBehaviourPunCallbacks
{
    public Text text;
    public GameObject button;


     List<string> nickList;

    [SerializeField] GameObject n1;
    [SerializeField] GameObject n2;
    [SerializeField] GameObject n3;
    [SerializeField] GameObject n4;
    
    [SerializeField] GameObject s1;
    [SerializeField] GameObject s2;
    [SerializeField] GameObject s3;
    [SerializeField] GameObject s4;

    int numPlayers;

    List<int> scoreList;
    string winner;
    GameObject panelStats;
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
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
