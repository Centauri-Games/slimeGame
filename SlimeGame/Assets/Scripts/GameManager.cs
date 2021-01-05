using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    PhotonView id;
    GameManager instance;
    public static bool gameStarted = false;

    //Timer
    double gameTimer = 50.0f;  //3 minutos
    double elapsedTime = 0.0f;
    double startTime;
    double remainTime;

    Text timer;

    //Players
    static List<string> playerNicks;
    static List<int> playersScore;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;

        id = GetComponent<PhotonView>();

        timer = GameObject.Find("Timer").GetComponent<Text>();
        Debug.Log(timer);

    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            Hashtable hash = new Hashtable();
            hash.Add("startTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        playerNicks = new List<string>();
        playersScore = new List<int>();
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            playerNicks.Add(p.NickName);
            playersScore.Add(0);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        base.OnPlayerEnteredRoom(newPlayer);
        playerNicks.Add(newPlayer.NickName);
        playersScore.Add(0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        int index = playerNicks.IndexOf(otherPlayer.NickName);
        playerNicks.RemoveAt(index);
        playersScore.RemoveAt(index);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (id.IsMine)
        {
            gameStarted = !gameStarted;
            if (gameStarted)
            {
                startTime = (double)propertiesThatChanged["startTime"];
            }
            else
            {
                Debug.Log("Partida acabada");   //Cambiar a otra escena
                for(int i= 0; i<playersScore.Count; i++)
                {
                    Debug.Log(playerNicks[i] + ": " + playersScore[i]);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            elapsedTime = PhotonNetwork.Time - startTime;   //Tiempo transcurrido
            remainTime = gameTimer - elapsedTime;


            if (remainTime <= 0 && PhotonNetwork.IsMasterClient)
            {
                Hashtable hash = new Hashtable();
                hash.Add("startTime", startTime);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }
        }
    }

    void OnGUI()
    {
        if (id.IsMine && gameStarted)
        {
            timer.text = (int)remainTime / 60 + ":" + (((int)remainTime) % 60).ToString("00");
        }
    }

    public void UpdateScore(Player killer)
    {
        id.RPC("RPC_UpdateScore", RpcTarget.All, killer.NickName);
    }

    [PunRPC]
    private void RPC_UpdateScore(string killer)
    {
        int index = playerNicks.IndexOf(killer);    //Mas una muerte en el marcador
        playersScore[index]++;
    }
}
