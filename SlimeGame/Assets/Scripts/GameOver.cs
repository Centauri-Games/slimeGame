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

    [SerializeField] GameObject texto1;
    [SerializeField] GameObject texto2;
    [SerializeField] GameObject texto3;
    [SerializeField] GameObject texto4;
      [SerializeField] GameObject textoEarnedPoints;
    

    int numPlayers = 1;

    List<int> scoreList;
    string winner;
    GameObject panelStats;
    // Start is called before the first frame update
    void Start()
    {
        //numPlayers = (int)PhotonNetwork.LocalPlayer.CustomProperties["numplayers"];

        if(PhotonNetwork.LocalPlayer.NickName.Equals((string)PhotonNetwork.LocalPlayer.CustomProperties["winner"])){
            textoEarnedPoints.GetComponent<Text>().text = "+ 60";
        }else{
            textoEarnedPoints.GetComponent<Text>().text = "+ 10";
        }

        
        texto1.GetComponent<Text>().text = "#1 "+(string)PhotonNetwork.LocalPlayer.CustomProperties["score0"];
        if(numPlayers > 1 ){
        texto2.GetComponent<Text>().text = "#2 "+(string)PhotonNetwork.LocalPlayer.CustomProperties["score1"];
        if(numPlayers > 2){
             texto3.GetComponent<Text>().text = "#3 "+(string)PhotonNetwork.LocalPlayer.CustomProperties["score2"];
             if( numPlayers > 3){
                  texto2.GetComponent<Text>().text = "#4 "+(string)PhotonNetwork.LocalPlayer.CustomProperties["score3"];
             }
        }
        }
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
        GameObject.Find("AudioManager").GetComponent<AudioManager>().playMusic(3);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
