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

        if (id.IsMine)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Text>();

        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            Hashtable hash = new Hashtable();
            hash.Add("startTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        playerNicks = new List<string>();
        playersScore = new List<int>();
        foreach (Player p in PhotonNetwork.PlayerList)   //Esto funcionaria realmente porque el objeto ya estaría instanciado y la escena empezaria tras crearse la sala con todos los jugadores
        {                                               //Ahora falla porque en el JoinRoom aun no está creado el objeto, asi que los primeros clientes no tendr
            if (!playerNicks.Contains(p.NickName))
            {
                playerNicks.Add(p.NickName);
                playersScore.Add(0);
            }
        }
        id.RPC("RPC_AddPlayer", RpcTarget.All);


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
            object o;
            if (propertiesThatChanged.TryGetValue("startTime", out o))
            {
                gameStarted = true;
                startTime = (double)o;

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
                EndGame();
            }
        }
    }

    void OnGUI()
    {
        if (id.IsMine && gameStarted)
        {
            int min = (int)remainTime / 60;
            int secs = (((int)remainTime) % 60);
            if (min < 0)
            {
                min = 0;
            }
            if (secs < 0)
            {
                secs = 0;
            }
            timer.text = min + ":" + secs.ToString("00");
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

    void EndGame()
    {
        id.RPC("RPC_EndGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_EndGame()
    {
        gameStarted = false;
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Partida acabada");   //Cambiar a otra escena
            for (int i = 0; i < playersScore.Count; i++)
            {
                Debug.Log(playerNicks[i] + ": " + playersScore[i]);
            }

            PhotonNetwork.LoadLevel("GameOver");
        }
    }


    [PunRPC]
    private void RPC_AddPlayer(PhotonMessageInfo info)
    {
        if (!PhotonNetwork.NickName.Equals(info.Sender.NickName))   //Solo se añade si no es el mismo jugador
        {
            if (!playerNicks.Contains(info.Sender.NickName))
            {
                playerNicks.Add(info.Sender.NickName);
                playersScore.Add(0);
            }
        }

    }
}