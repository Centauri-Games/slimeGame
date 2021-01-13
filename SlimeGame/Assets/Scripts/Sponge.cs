using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Sponge : Gun
{
    float radius = 2f;
    public GameObject waterExplosion;

    bool isFixed = false;

    PhotonView id;

    void Awake()
    {
        id = GetComponent<PhotonView>();
        if (!id.IsMine)
        {
            Destroy(GetComponent<Rigidbody>());
        }
    }
    void Start()
    {
        GameManager.am.playSound(5, 1f);
    }
    public override void Use()
    {

    }

    void Explode()
    {

        //Show particles water
        PhotonNetwork.Instantiate(Path.Combine("Bubbles", "Prefabs", "Bubbles"), transform.position, Quaternion.Euler(-90f, 0f, 0f));
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
        //Destroy gameobject
        //Destroy gameobject
        id.RPC("RPC_Destroy", RpcTarget.All);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (id.IsMine)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (isFixed)
                {
                    Explode();
                }
            }
            else
            {
                if (!isFixed)
                {
                    isFixed = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
    [PunRPC]
    void RPC_Destroy()
    {
        GameManager.am.playSound(3,1f);    //Sonido splash
        GameManager.am.playSound(2, 0.2f);    //Sonido pompas
        Destroy(gameObject);
    }
    public override void End() { }

}
