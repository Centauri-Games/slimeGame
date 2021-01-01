using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : Gun
{
    [SerializeField] ParticleSystem waterJet;

    public void Start()
    {
        waterJet.Stop();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        waterJet.Play();
    }


    public override void End()
    {
        waterJet.Stop();
    }

    private void OnEnable()
    {
        waterJet.Stop();
    }
}
