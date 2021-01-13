using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterGun : Gun
{
    [SerializeField] ParticleSystem waterJet;
    [SerializeField] List<GameObject> parts;
    [SerializeField] List<Material> materials;
    [SerializeField] public static float ammo;
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
            ammoBar = new Rect(Screen.width / 20 + 20, Screen.height * 4 / 30, Screen.width / 3, Screen.height / 50);
            ammoTex = new Texture2D(1, 1);
            ammoTex.SetPixel(0, 0, Color.cyan);
            ammoTex.Apply();
        }
    }
    public void Start()
    {
        Stop();

        int skin = (int)id.Owner.CustomProperties["waterGunSkin"];

        switch (skin)
        {
            case 0:
                //No hace nada, skin por defecto
                break;
            case 1: //Blue cammo
                parts[0].GetComponent<Renderer>().material = materials[0];
                parts[1].GetComponent<Renderer>().material = materials[1];
                parts[2].GetComponent<Renderer>().material = materials[1];
                parts[3].GetComponent<Renderer>().material = materials[1];
                parts[4].GetComponent<Renderer>().material = materials[1];
                parts[5].GetComponent<Renderer>().material = materials[0];
                break;
            case 2: //Red cammo
                parts[0].GetComponent<Renderer>().material = materials[0];
                parts[1].GetComponent<Renderer>().material = materials[2];
                parts[2].GetComponent<Renderer>().material = materials[2];
                parts[3].GetComponent<Renderer>().material = materials[2];
                parts[4].GetComponent<Renderer>().material = materials[2];
                parts[5].GetComponent<Renderer>().material = materials[0];
                break;
            default:
                break;
        }
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        if (ammo > 0)
        {
            GameManager.am.playSoundLoop(8, 1f);
            waterJet.Play();
            isShooting = true;
        }
    }

    private void FixedUpdate()
    {
        if (ShowOptions.IsPaused() && isShooting)
        {
            id.RPC("RPC_Stop", RpcTarget.All);
        }
        if (isShooting)
        {
            if (id.IsMine)
            {
                ammo -= 0.2f;   //La municion la actualizan todos
                if (ammo < 0)   //Solo el jugador que dispara indica al resto que paren de disparar
                {
                    GameManager.am.stopSoundLoop();
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
