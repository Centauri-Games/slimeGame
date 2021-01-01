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

    public override void Use()
    {

    }

    void Explode()
    {
        //Show particles water
        PhotonNetwork.Instantiate(Path.Combine("SimpleFX", "Prefabs", "FX_BlueExplosion"), transform.position, transform.rotation);
        //Check nearby objects 
        //If player-> take damage
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Player") && c.GetType()!=typeof(CharacterController))
            {
                c.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            }
        }
        //Destroy gameobject
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
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
            if (!isFixed) {
                isFixed = true;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    public override void End(){}
}
