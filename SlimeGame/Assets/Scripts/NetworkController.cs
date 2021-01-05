using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NetworkController : MonoBehaviour//PunCallbacks
{
    
    void Start()
    {
        /*if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Cliente master");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameManager"), Vector3.zero, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AmmoSpawner"), new Vector3(-26.5f, 5.3f, -22.5f), Quaternion.identity);
        }
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);*/

        /*if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                Debug.Log("\nConectado al servidor");
            }
            else
            {
                Debug.Log("\nSe ha producido un error de conexión");
            }
        }*/
    }

   /* public override void OnConnectedToMaster()      //ToDO: Toda la logica de este script meterla en las salas (menu) y el instantiate usarlo en RoomManager
    {
        Debug.Log("Connected to server: " + PhotonNetwork.CloudRegion);
        if (!PhotonNetwork.JoinRandomRoom())
        {
           Debug.Log("\nHa ocurrido un error al unirse a la sala");
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("\nNo existen salas a las que unirse, creando una nueva...");

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }))
        {
            Debug.Log("\nSala creada con éxito");
        }
        else
        {
            Debug.Log("\nHa ocurrido un error durante la creación de la sala");
        }
    }

    public override void OnJoinedRoom()
    {   
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameManager"), Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Cliente master");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AmmoSpawner"), new Vector3(-26.5f, 5.3f, -22.5f), Quaternion.identity);
        }
    }

   */
}

