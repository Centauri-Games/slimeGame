using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameManager : MonoBehaviour
{

    PhotonView id;

    // Start is called before the first frame update
    private void Awake()
    {
        id = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAmmo()
    {
        
    }
}
