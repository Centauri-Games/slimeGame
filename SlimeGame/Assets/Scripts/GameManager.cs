using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    PhotonView id;
    GameManager instance;
    public static bool gameStarted = false;

    //Timer
    double gameTimer = 180.0f;  //3 minutos
    double elapsedTime = 0.0f;
    double startTime;
    double remainTime = 180.0f;

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

    string nick1;

    public static AudioManager am;

    //Equipos
    bool teamBattle = false;
    static List<int> playerTeams;

    GameObject statsButton;
    bool mobile = false;

    // Start is called before the first frame update
    private void Awake()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (instance == null)
            instance = this;

        id = GetComponent<PhotonView>();

        if (id.IsMine)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        mobile = MobileChecker.isMobile();

        if (SceneManager.GetActiveScene().name.Equals("SampleScene"))
        {
            am.playMusic(0);
        }
        else if (SceneManager.GetActiveScene().name.Equals("Vestuario"))
        {
            am.playMusic(1);
        }
        else if (SceneManager.GetActiveScene().name.Equals("Piscina"))
        {
            am.playMusic(2);
        }

    }

    void Start()
    {
        if (id.IsMine)
        {
            if (!((bool)PhotonNetwork.CurrentRoom.CustomProperties["Deathmatch"]) && PhotonNetwork.CurrentRoom.MaxPlayers == 1) //Si deathmatch es false y hay 4 jugadores -> Pelea de equipos
            {
                teamBattle = true;
            }
            stats = GameObject.Find("Stats");

            if (mobile)
            {
                statsButton = GameObject.Find("StatButton");
                statsButton.GetComponent<Button>().onClick.AddListener(showStats);
            }

            n1 = GameObject.Find("nickname1").GetComponent<Text>();
            n2 = GameObject.Find("nickname2").GetComponent<Text>();
            n3 = GameObject.Find("nickname3").GetComponent<Text>();
            n4 = GameObject.Find("nickname4").GetComponent<Text>();

            s1 = GameObject.Find("score1").GetComponent<Text>();
            s2 = GameObject.Find("score2").GetComponent<Text>();
            s3 = GameObject.Find("score3").GetComponent<Text>();
            s4 = GameObject.Find("score4").GetComponent<Text>();

            stats.SetActive(false);
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
            playerTeams = new List<int>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (!playerNicks.Contains(p.NickName))
                {
                    playerNicks.Add(p.NickName);
                    playersScore.Add(0);

                    if (teamBattle)
                    {
                        playerTeams.Add((int)p.CustomProperties["teamIndex"]);
                    }
                }
            }
            id.RPC("RPC_AddPlayer", RpcTarget.All);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (id.IsMine)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            int index = playerNicks.IndexOf(otherPlayer.NickName);
            playerNicks.RemoveAt(index);
            playersScore.RemoveAt(index);

            if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            {
                EndGame();
            }
        }
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
        if (id.IsMine)
        {
            if (!mobile)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    showStats();

                }
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    if (stats.active)
                    {
                        stats.SetActive(false);
                    }
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
    }

    public void showStats()
    {
        if (mobile)
        {
            if (stats.active)
            {
                stats.SetActive(false);
                return;
            }

        }
        if (!stats.active)
        {
            stats.SetActive(true);
        }

        n1.text = "#1 : " + playerNicks[0];
        s1.text = "" + playersScore[0];
        if (playerNicks.Count > 1)
        {
            n2.text = "#2 : " + playerNicks[1];
            s2.text = "" + playersScore[1];
        }
        if (playerNicks.Count > 2)
        {
            n3.text = "#3 : " + playerNicks[2];
            s3.text = "" + playersScore[2];
            if (playerNicks.Count > 3)
            {
                n4.text = "#4 : " + playerNicks[3];
                s4.text = "" + playersScore[3];
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
        if (teamBattle)
        {
            if ((int)killer.CustomProperties["teamIndex"] != (int)PhotonNetwork.LocalPlayer.CustomProperties["teamIndex"])  //Si no son del mismo equipo, aumenta puntuación
                id.RPC("RPC_UpdateScore", RpcTarget.All, killer.NickName);
        }
        else
        {
            id.RPC("RPC_UpdateScore", RpcTarget.All, killer.NickName);
        }
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
        for (int i = 0; i < playersScore.Count - 1; i++)
        {
            if (playersScore[i] < playersScore[i + 1])
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Hashtable h = new Hashtable();
        for (int i = 0; i < playerNicks.Count; i++)
        {
            h.Add("score" + i, "" + playerNicks[i] + " : " + playersScore[i]);
        }
        h.Add("numplayers", playerNicks.Count);
        h.Add("winner", playerNicks[0]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
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

    //prueba

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