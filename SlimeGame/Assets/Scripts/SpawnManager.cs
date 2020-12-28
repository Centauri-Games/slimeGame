using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    Spawnpoint[] spawns;

    void Awake()
    {
        Instance = this;
        spawns = GetComponentsInChildren<Spawnpoint>();
    }

    public Transform GetSpawnpoint()
    {
        return spawns[Random.Range(0, spawns.Length)].transform;    //Devuelve un punto de aparicion aleatorio
    }
}
