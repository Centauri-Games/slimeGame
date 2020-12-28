using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : Gun
{
    public override void Use()
    {
        Debug.Log("Using gun " + itemInfo.itemName);
        Shoot();
    }

    void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = transform.position;    //Origen en el arma

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We hit " + hit.collider.gameObject.name);

            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
        }
    }
}
