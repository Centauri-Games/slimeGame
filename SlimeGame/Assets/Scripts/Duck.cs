﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class Duck : MonoBehaviourPunCallbacks
{
    float radius = 3f;
    public GameObject waterExplosion;
    public ItemInfo itemInfo;

    float elapsedTime = 0f;
    int remainingTicks = 5;
    float tickRate = 1f;

    bool hasExploded = false;

    PhotonView id;

    void Awake()
    {
        id = GetComponent<PhotonView>();
        if (!id.IsMine)
        {
            Debug.Log("No es mio");
            Destroy(GetComponent<Rigidbody>());
        }
    }
    private void Update()
    {
        if (id.IsMine)
        {
            if (hasExploded)    //Check dmg every sec for hp reduction
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= tickRate)
                {

                    PhotonNetwork.Instantiate(Path.Combine("SimpleFX", "Prefabs", "FX_BlueExplosion"), transform.position, transform.rotation);    //Al explotar activa la deteccion de daño y las particulas
                    elapsedTime = 0.0f;
                    //Check nearby objects 
                    //If player-> take damage
                    Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                    foreach (Collider c in colliders)
                    {
                        if (c.gameObject.CompareTag("Player") && c.GetType() != typeof(CharacterController))
                        {
                            c.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                        }
                    }

                    remainingTicks--;
                    if (remainingTicks <= 0)
                    {
                        //Destroy gameobject
                        id.RPC("RPC_Destroy", RpcTarget.All);
                    }
                }
            }
        }

    }

    void Explode()
    {
        
        if (id.IsMine)
        {
            //Show particles water
            PhotonNetwork.Instantiate(Path.Combine("SimpleFX", "Prefabs", "FX_BlueExplosion"), transform.position, transform.rotation);    //Al explotar activa la deteccion de daño y las particulas
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider c in colliders)
            {
                if (c.gameObject.CompareTag("Player") && c.GetType() != typeof(CharacterController))
                {
                    c.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                }
            }

            //Update explosion status
            hasExploded = true;
            Hashtable hash = new Hashtable();
            hash.Add("explosionStatus", hasExploded);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!id.IsMine && targetPlayer == id.Owner)   //Si es el jugador que ha cambiado el arma y no es el local
        {
            hasExploded = (bool)changedProps["explosionStatus"];
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (id.IsMine)
        {
            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Wall"))
            {
                if (!hasExploded)       //Cuando detecta un objeto que no sea el jugador o una pared, explota
                {
                    GetComponent<Rigidbody>().isKinematic = true;
                    Explode();
                }
            }
        }
    }

    [PunRPC]
    void RPC_Destroy()
    {
        Destroy(gameObject);
    }
}
