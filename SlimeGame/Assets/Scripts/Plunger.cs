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
    [SerializeField] List<Material> materials;
    Ray ray;

    PhotonView id;

    float currentWait;
    float delay = 0.5f;
    bool used = false;

    [SerializeField] Animator plungerController;
    void Start()
    {
        //attackAnim = plunger.GetComponent<Animator>();
        currentWait = 0.0f;

        id = itemGameObject.GetComponent<PhotonView>();


        int skin = (int)id.Owner.CustomProperties["plungerSkin"];

        Renderer r = plunger.GetComponent<Renderer>();
        switch (skin)
        {
            case 0:
                //No hace nada, skin por defecto
                break;
            case 1:
                r.material = materials[0];
                break;
            case 2:
                r.material = materials[1];
                break;
            default:
                break;
        }


    }

    void Update()
    {
        if (used)
        {
            currentWait += Time.deltaTime;
            if (currentWait >= delay)
            {
                plungerController.SetBool("Attacking", false);
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
            ray = new Ray(currentPlayer.transform.position, currentPlayer.transform.forward * 1.2f);
            plungerController.SetBool("Attacking", true);
           
            if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Weapons", "FX", "HitEffect"), hit.point, transform.rotation);
                if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject != currentPlayer)
                {
                    PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Weapons", "FX", "HitEffect"), hit.point, transform.rotation);
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                }
            }
            HitSound();
        }
    }

    public override void End()
    { //plungerController.SetBool("Attacking", false); 
    }

    void HitSound()
    {
        GameManager.am.playSound(1,1f);
    }
}
