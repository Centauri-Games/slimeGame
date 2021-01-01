﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class WaterBalloon : Gun
{
    public float delay = 10f;
    float radius = 3f;

    public GameObject waterExplosion;

    float countdown;
    bool hasExploded = false;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0 && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }
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

    public override void End(){}
}
