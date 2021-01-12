using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Plunger : Gun
{
    Animator attackAnim;
    [SerializeField] GameObject plunger;
    [SerializeField] GameObject currentPlayer;
    [SerializeField] float attackRange = 1.5f;
    Ray ray;

    float currentWait;
    float delay = 0.5f;
    bool used = false;
    void Start()
    {
        //attackAnim = plunger.GetComponent<Animator>();

        currentWait = 0.0f;
    }

    void Update()
    {
        if (used)
        {
            currentWait += Time.deltaTime;
            if (currentWait >= delay)
            {
                currentWait = 0.0f;
                used = false;
            }
        }
    }
    public override void Use()
    {
        if (!used)
        {
            used = true;
            //Play animation hitting

            //Raycast para detectar si está delante y cerca
            ray = new Ray(currentPlayer.transform.position, currentPlayer.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Weapons", "FX", "HitEffect"), hit.point, transform.rotation);
                if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject != currentPlayer)
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Weapons", "FX", "HitEffect"), hit.point, transform.rotation);
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                }
            }
        }
    }

    public override void End() { }  //Vacío
}
