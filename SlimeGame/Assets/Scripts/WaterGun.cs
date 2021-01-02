using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterGun : Gun
{
    [SerializeField] ParticleSystem waterJet;
    [SerializeField] float ammo;
    [SerializeField] float totalAmmo = 100f;
    bool isShooting = false;

    PhotonView id;

    //UI
    Rect ammoBar;
    Texture2D ammoTex;

    void Awake()
    {
        id = GetComponent<PhotonView>();
        ammo = totalAmmo;

        if (id.IsMine)  //Para evitar multiples instancias de la barra de municion
        {
            ammoBar = new Rect(Screen.width / 10, Screen.height * 17 / 20, Screen.width / 3, Screen.height / 50);
            ammoTex = new Texture2D(1, 1);
            ammoTex.SetPixel(0, 0, Color.cyan);
            ammoTex.Apply();
        }
    }
    public void Start()
    {
        Stop();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        if (ammo > 0)
        {
            waterJet.Play();
            isShooting = true;
        }
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            ammo -= 0.2f;   //La municion la actualizan todos
            if (id.IsMine)
            {
                if (ammo < 0)   //Solo el jugador que dispara indica al resto que paren de disparar
                {
                    id.RPC("RPC_Stop", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void RPC_Recharge()
    {
        ammo = totalAmmo;
        Debug.Log(ammo);
    }

    public void Recharge()
    {
        Debug.Log("Recarga");
        id.RPC("RPC_Recharge", RpcTarget.All);
    }


    public override void End()
    {
        Stop();
    }

    public void Stop()
    {
        isShooting = false;
        waterJet.Stop();
    }

    [PunRPC]
    void RPC_Stop()
    {
        Stop();
    }

    void OnGUI()
    {
        if (id.IsMine)
        {
            float ratio = ammo / totalAmmo;
            float barWidth = ratio * Screen.width / 3;
            ammoBar.width = barWidth;
            GUI.DrawTexture(ammoBar, ammoTex);
        }
    }

}
