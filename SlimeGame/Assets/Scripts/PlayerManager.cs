using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{

    PhotonView view;
    GameObject controller;

    PhotonView gmID;

    void Awake()
    {
        view = GetComponent<PhotonView>();

    }

    void Start()
    {
        if (view.IsMine)    //Si es el jugador local
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform sp = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), sp.position, sp.rotation, 0, new object[] { view.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);

        CreateController();
    }

}
