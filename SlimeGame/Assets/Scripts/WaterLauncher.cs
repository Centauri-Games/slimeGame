using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLauncher : Gun
{
    [SerializeField] float throwForce = 40f;
    [SerializeField] GameObject[] grenades;

    int currentType = 2;

    public override void End(){}

    public override void Use()
    {
        ThrowGrenade();
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenades[currentType], transform.position + new Vector3(0f,0.5f, 0.5f), transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }
}
