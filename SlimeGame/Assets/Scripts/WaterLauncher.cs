using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class WaterLauncher : Gun
{
    [SerializeField] float throwForce = 40f;
    [SerializeField] GameObject charger;
    [SerializeField] List<Material> materials;
    string[] grenades = new string[3];

    int currentType = 1;

    PhotonView id;

    void Awake()
    {
        id = GetComponent<PhotonView>();
        grenades[0] = Path.Combine("Prefabs", "Weapons", "waterBalloon test");
        grenades[1] = Path.Combine("Prefabs", "Weapons", "sponge Test");
        grenades[2] = Path.Combine("Prefabs", "Weapons", "Duck Test");

    }

    void Start()
    {
        currentType = (int)PhotonNetwork.LocalPlayer.CustomProperties["grenadeIndex"];

        int skin = (int)id.Owner.CustomProperties["waterGrenadeSkin"];

        Renderer r = charger.GetComponent<Renderer>();
        switch (skin)
        {
            case 0:
                //No hace nada, skin por defecto
                break;
            case 1:
                r.material = materials[0];  //Blue cammo
                break;
            case 2:
                r.material = materials[1]; //Red cammo
                break;
            default:
                break;
        }
    }
    public override void End() { }

    public override void Use()
    {
        Debug.Log("Lanzando granada");
        ThrowGrenade();

    }

    void ThrowGrenade()
    {
        GameObject grenade = PhotonNetwork.Instantiate(grenades[currentType], transform.position + new Vector3(0f, 0.2f, 0.8f), transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
    }
}
