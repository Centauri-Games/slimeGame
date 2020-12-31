using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : Gun
{
    Animator attackAnim;
    [SerializeField] GameObject plunger;
    [SerializeField] GameObject currentPlayer;
    [SerializeField] float attackRange = 1.5f;
    Ray ray;
    void Start()
    {
        //attackAnim = plunger.GetComponent<Animator>();
        ray = new Ray();
    }

    void Update()
    {
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
    public override void Use()
    {
        //Play animation hitting
        Debug.Log("Using gun " + itemInfo.itemName);

        //Raycast para detectar si está delante y cerca
        ray = new Ray(currentPlayer.transform.position, currentPlayer.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            Debug.Log("Alguien");
            if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject!=currentPlayer)
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            }
        }
    }

    public override void End() { }  //Vacío
}
