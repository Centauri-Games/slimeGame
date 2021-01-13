using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Lobby : MonoBehaviourPunCallbacks
{

    public Button ConnectBtn;
    public Button JoinRandomBtn;
    public Text Log;

    [SerializeField] public Button start;
    bool deathmatch;

    [SerializeField] public GameObject[] textList;
    public byte maxPlayersInRoom = 4;
    public byte minPlayersInRoom = 2;

    public int playerCounter;
    public Text PlayerCounter;
    bool loadReady = true;

    [SerializeField] public Button duck;
    [SerializeField] public Button waterBallon;
    [SerializeField] public Button sponge;
    public Hashtable customGrenadePlayerProperties;

    public Hashtable customSkinsProperties;

    Hashtable deathmachTrue = new Hashtable() { { "Deathmatch", true } };
    Hashtable deathmachFalse = new Hashtable() { { "Deathmatch", false }};
    public void Start()
    {
        //Log.text += "\nServidor: " + PhotonNetwork.CloudRegion;

        setGrenade(PlayerPrefs.GetInt("grenadeIndex", 0));

        

    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectToRegion("eu"))
            {
                if (PlayerPrefs.GetInt("language") == 1)
                {
                    Log.text += "\nConectado al servidor";
                }
                else
                {
                    Log.text = "\nConnected to server";
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("language") == 1)
                {
                    Log.text += "\nSe ha producido un error de conexión";
                }
                else
                {
                    Log.text += "\nA connection error has occured";
                }
            }
        }
    }
    public void setGrenade(int grenadeIndex)
    {
        PlayerPrefs.SetInt("grenadeIndex", grenadeIndex);
        customGrenadePlayerProperties = new Hashtable();
        switch (grenadeIndex)
        {
            case 0:
                duck.interactable = false;
                waterBallon.interactable = true;
                sponge.interactable = true;

                customGrenadePlayerProperties.Add("grenadeIndex", 0);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customGrenadePlayerProperties);
                break;
            case 1:
                duck.interactable = true;
                waterBallon.interactable = true;
                sponge.interactable = false;
                customGrenadePlayerProperties.Add("grenadeIndex", 1);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customGrenadePlayerProperties);
                break;
            case 2:
                duck.interactable = true;
                waterBallon.interactable = false;
                sponge.interactable = true;
                customGrenadePlayerProperties.Add("grenadeIndex", 2);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customGrenadePlayerProperties);
                break;
        }
    }
    public override void OnConnectedToMaster()
    {
        if (PlayerPrefs.GetInt("language") == 1)
        {
            Log.text += "\nServidor: " + PhotonNetwork.CloudRegion;
        }
        else
        {
            Log.text += "\nServer: " + PhotonNetwork.CloudRegion;
        }

        JoinRandomBtn.interactable = true;
    }

    public void JoinRandom2Players()
    {
        maxPlayersInRoom = 2;
        Debug.Log("\nServidor: " + PhotonNetwork.CloudRegion);
        deathmatch = false;
        if (!PhotonNetwork.JoinRandomRoom(deathmachFalse, 2))
        {
            if (PlayerPrefs.GetInt("language") == 1)
            {
                Log.text += "\nHa ocurrido un error al unirse a la sala";
            }
            else
            {
                Log.text += "\nAn error occured while entering the room";
            }
        }
    }

    public void JoinRandom4Players()
    {
        //Connect();
        maxPlayersInRoom = 4;
        deathmatch = false;
        if (!PhotonNetwork.JoinRandomRoom(deathmachFalse, 4))
        {
            if (PlayerPrefs.GetInt("language") == 1)
            {
                Log.text += "\nHa ocurrido un error al unirse a la sala";
            }
            else
            {
                Log.text += "\nAn error occured while entering the room";
            }
        }
    }

    public void JoinRandomDeathmatch()
    {
        maxPlayersInRoom = 4;
        deathmatch = true;
        if (!PhotonNetwork.JoinRandomRoom(deathmachTrue, 4))
        {
            if (PlayerPrefs.GetInt("language") == 1)
            {
                Log.text += "\nHa ocurrido un error al unirse a la sala";
            }
            else
            {
                Log.text += "\nAn error occured while entering the room";
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
       Debug.Log("\nNo existen salas a las que unirse, creando una nueva...");

        if (deathmatch)
        {
            RoomOptions op = new RoomOptions();
            op.MaxPlayers = maxPlayersInRoom;
            op.IsOpen = true;
            op.IsVisible = true;
            op.CustomRoomProperties = deathmachTrue;
            op.CustomRoomPropertiesForLobby = new string[] { "Deathmatch" };

            if (PhotonNetwork.CreateRoom(null, op, null, null))
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(deathmachTrue);
                Debug.Log("\nSala creada con éxito");
            }
            else
            {
                Debug.Log("\nHa ocurrido un error durante la creación de la sala");
            }
        }
        else
        {
            RoomOptions op = new RoomOptions();
            op.MaxPlayers = maxPlayersInRoom;
            op.IsOpen = true;
            op.IsVisible = true;
            op.CustomRoomProperties = deathmachFalse;
            op.CustomRoomPropertiesForLobby = new string[] { "Deathmatch" };

            if (PhotonNetwork.CreateRoom(null, op, null, null))
            {               
                Debug.Log("\nSala creada con éxito");
            }
            else
            {
                Debug.Log("\nHa ocurrido un error durante la creación de la sala");
            }
        }
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PlayerPrefs.GetInt("language") == 1)
        {
            Log.text += "\nSe ha unido a la sala";
        }
        else
        {
            Log.text += "\nYou have joined the room";
        }

        //JoinRandomBtn.interactable = false;
        customSkinsProperties = new Hashtable();
        customSkinsProperties.Add("slimeSkin",PlayerPrefs.GetInt("slimeSkin"));
        customSkinsProperties.Add("waterGunSkin",PlayerPrefs.GetInt("waterGunSkin"));
        customSkinsProperties.Add("waterGrenadeSkin",PlayerPrefs.GetInt("waterGrenadeSkin"));
        customSkinsProperties.Add("plungerSkin",PlayerPrefs.GetInt("plungerSkin"));
        customSkinsProperties.Add("teamIndex", -1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(customSkinsProperties);
        PhotonNetwork.AutomaticallySyncScene = true;


    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Text auxText;
        base.OnPlayerLeftRoom(otherPlayer);
        for (int i = 0; i < textList.Length; i++)
        {
            auxText = textList[i].GetComponent<Text>();
            if (auxText.text.Equals(otherPlayer.NickName))
            {
                if (PlayerPrefs.GetInt("language") == 1)
                {
                    auxText.text = "Esperando a otro jugador...";
                }
                else
                {
                    auxText.text = "Waiting for other player...";
                }
                start.gameObject.SetActive(false);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    public void setTeam()
    {
        int playersTeam1 = 0;
        int playersTeam2 = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int team = Random.Range(0, 2);

            switch (team)
            {
                case 0: //Equipo 1
                    if (playersTeam1 < 2)
                    {
                        //Añade al equipo 1
                        PhotonNetwork.PlayerList[i].CustomProperties["teamIndex"] = 0;
                        playersTeam1++;
                    }
                    else
                    {
                        PhotonNetwork.PlayerList[i].CustomProperties["teamIndex"] = 1;
                        playersTeam2++;
                        //Añade al equipo 2
                    }
                    break;

                case 1: //Equipo 2
                    if (playersTeam2 < 2)
                    {
                        PhotonNetwork.PlayerList[i].CustomProperties["teamIndex"] = 1;
                        //Añade al equipo 2
                        playersTeam2++;
                    }
                    else
                    {
                        PhotonNetwork.PlayerList[i].CustomProperties["teamIndex"] = 0;
                        //Añade al equipo 1
                        playersTeam1++;
                    }

                    break;
            }
        }

    }

    public void startGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            if (!deathmatch && PhotonNetwork.CurrentRoom.MaxPlayers == 1)
            {
                setTeam();  //Organiza los equipos
            }

            int n = Random.Range(0, 3);
            switch (n)
            {
                case 0:
                    PhotonNetwork.LoadLevel("SampleScene");
                    break;
                case 1:
                    PhotonNetwork.LoadLevel("Vestuario");
                    break;
                case 2:
                    PhotonNetwork.LoadLevel("Piscina");
                    break;
                default:
                    break;

            }

        }
    }
    public void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playerCounter = PhotonNetwork.CurrentRoom.PlayerCount;

            for (int i = 0; i < playerCounter; i++)
            {


                textList[i].GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
            }
            Debug.Log("Player Count" + PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersInRoom && PhotonNetwork.IsMasterClient && !start.IsActive())
            {
                start.gameObject.SetActive(true);
                /*if (loadReady)
                {   
                     PhotonNetwork.CurrentRoom.IsOpen = false;
                     PhotonNetwork.LoadLevel("SampleScene");
                    loadReady = false;
                }*/
            }
        }

        PlayerCounter.text = playerCounter + "/" + maxPlayersInRoom;
    }
}
