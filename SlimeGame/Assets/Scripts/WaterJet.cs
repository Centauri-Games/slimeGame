using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterJet : MonoBehaviour
{
    private ParticleSystem waterJet;
    private List<ParticleCollisionEvent> collisionEvents;
    [SerializeField] GameObject currentPlayer;
    [SerializeField] WaterGun wg;

    PhotonView playerId;

    void Start()
    {
        waterJet = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        playerId = currentPlayer.GetComponent<PhotonView>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (playerId.IsMine)    //Solo comprueba si le ha dado el jugador local, para que no se duplique la colision
        {
            if (other.tag == "Player" && other != currentPlayer)  //Si el jugador alcanzado no es el mio 
            {
                Debug.Log("me reventaste el orto putooo");
                int collCount = waterJet.GetSafeCollisionEventSize();
                int eventCount = waterJet.GetCollisionEvents(other, collisionEvents);

                for (int i = 0; i < eventCount; i++)
                {
                    other.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)wg.itemInfo).damage);
                }
            }
        }
    }
}
