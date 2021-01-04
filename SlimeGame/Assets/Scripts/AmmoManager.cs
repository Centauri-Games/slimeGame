using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class AmmoManager : MonoBehaviour
{

    PhotonView id;
    public static AmmoManager Instance;
    Spawnpoint[] spawns; //GameObjects de los puntos de municion
    bool[] ammoSpawned; //Array que indica si punto de municion ocupado

    void Awake()
    {
        Instance = this;
        id = GetComponent<PhotonView>();
    }

    private void Start()
    {
        spawns = GetComponentsInChildren<Spawnpoint>();
        ammoSpawned = new bool[spawns.Length];
    }


    void spawnAmmo(int pos)
    {
        if (!ammoSpawned[pos])
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Item", "Ammo"), spawns[pos].transform.position, spawns[pos].transform.rotation,
                0, new object[] { id.ViewID, pos }); //Spawn de municion

            ammoSpawned[pos] = true;
        }
    }

    void restoreAmmo()
    {
        for (int i = 0; i < ammoSpawned.Length; i++)
        {
            if (!ammoSpawned[i])
            {
                spawnAmmo(i);
                return;
            }
        }
    }

    public void ammoDestroyed(int pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Ammo destroyed");
            ammoSpawned[pos] = false;
            Invoke("restoreAmmo", 3);
        }
    }
}
