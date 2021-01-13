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
    List<GameObject> textList;

    Text n1;
    Text n2;
    Text n3;
    Text n4;

    Text s1;

    Text s2;

    Text s3;

    Text s4;

    GameObject stats;
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

         stats = GameObject.Find("Stats");
        
        n1 = GameObject.Find("nickname1").GetComponent<Text>();
        n2 = GameObject.Find("nickname2").GetComponent<Text>();
        n3 = GameObject.Find("nickname3").GetComponent<Text>();
        n4 = GameObject.Find("nickname4").GetComponent<Text>();

        s1 = GameObject.Find("score1").GetComponent<Text>();
        s2 = GameObject.Find("score2").GetComponent<Text>();
        s3 = GameObject.Find("score3").GetComponent<Text>();
        s4 = GameObject.Find("score4").GetComponent<Text>();


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

        foreach (Player p in PhotonNetwork.PlayerList)   
        {                                               
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

                if(Input.GetKeyDown(KeyCode.Tab)){
            if(!stats.active){
                stats.SetActive(true);
            }

            n1.text = "#1 : "+ playerNicks[0];
            n2.text = "#2 : "+ playerNicks[1];
            if(playerNicks.Count > 2){
                n3.text = "#3 : "+ playerNicks[2];
                n4.text = "#4 : "+ playerNicks[3];
            }
            

            s1.text = "" +playersScore[0];
            s2.text = "" +playersScore[1];
            if(playersScore.Count > 2){
                s3.text = "" +playersScore[2];
                s4.text = "" +playersScore[3];
            }
            
        }
        if(Input.GetKeyUp(KeyCode.Tab)){
            if(stats.active){
                stats.SetActive(false);
            }
        }
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

        orderScore();
    }

    private void orderScore()
    {
        for (int i = 0; i < playersScore.Count-1; i++)
        {
            if(playersScore[i] < playersScore[i + 1])
            {
                int score = playersScore[i];    //Guardar original
                string name = playerNicks[i];

                playersScore[i] = playersScore[i + 1];  //reemplazar siguiente por actual
                playerNicks[i] = playerNicks[i + 1];

                playersScore[i + 1] = score;    //recolocar
                playerNicks[i + 1] = name;
            }
        }
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