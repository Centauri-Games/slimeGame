using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{

    public Button ConnectBtn;
    public Button JoinRandomBtn;
    public Text Log;

    public byte maxPlayersInRoom = 4;
    public byte minPlayersInRoom = 2;

    public int playerCounter;
    public Text PlayerCounter;
    bool loadReady = true;

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                Log.text += "\nConectado al servidor";
            }
            else
            {
                Log.text += "\nSe ha producido un error de conexión";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Log.text += "\nServidor: " + PhotonNetwork.CloudRegion;
        ConnectBtn.interactable = false;
        JoinRandomBtn.interactable = true;
    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nHa ocurrido un error al unirse a la sala";
        }
    }

    public void JoinRandom4Players()
    {
        //Connect();
        maxPlayersInRoom = 2;
        if (!PhotonNetwork.JoinRandomRoom())
        {
            Log.text += "\nHa ocurrido un error al unirse a la sala";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Log.text += "\nNo existen salas a las que unirse, creando una nueva...";

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersInRoom }))
        {
            Log.text += "\nSala creada con éxito";
        }
        else
        {
            Log.text += "\nHa ocurrido un error durante la creación de la sala";
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Log.text += "\nSe ha unido a la sala";
        JoinRandomBtn.interactable = false;

        PhotonNetwork.AutomaticallySyncScene = true;


    }


    public void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playerCounter = PhotonNetwork.CurrentRoom.PlayerCount;
            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersInRoom && PhotonNetwork.IsMasterClient)
            {
                if (loadReady)
                {
                    PhotonNetwork.LoadLevel("SampleScene");
                    loadReady = false;
                }
            }
        }

        PlayerCounter.text = playerCounter + "/" + maxPlayersInRoom;
    }
}
