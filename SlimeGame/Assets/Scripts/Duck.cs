using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Duck : Gun
{
    float radius = 3f;
    public GameObject waterExplosion;

    float elapsedTime= 0f;
    int remainingTicks = 5;
    float tickRate = 1f;

    bool hasExploded = false;

    public override void Use(){}

    private void Update()
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
                if (remainingTicks <=0)
                {
                    //Destroy gameobject
                    Destroy(gameObject);
                }
            }
        }

    }

    void Explode()
    {
        //Show particles water
        PhotonNetwork.Instantiate(Path.Combine("SimpleFX","Prefabs","FX_BlueExplosion"), transform.position, transform.rotation);    //Al explotar activa la deteccion de daño y las particulas
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Player") && c.GetType() != typeof(CharacterController))
            {
                c.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            }
        }
        hasExploded = true;
       
    }

    private void OnCollisionEnter(Collision collision)
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
    public override void End(){}
}
