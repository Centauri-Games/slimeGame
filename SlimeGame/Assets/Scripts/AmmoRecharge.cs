using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoRecharge : MonoBehaviour
{
    Vector3 rot = new Vector3(0f, 1f, 0.0f);
    PhotonView id;
    AmmoManager am;
    int position;

    void Awake()
    {
        id = GetComponent<PhotonView>();
        am = PhotonView.Find((int)id.InstantiationData[0]).GetComponent<AmmoManager>(); //Get Ammomanager
        position = (int)id.InstantiationData[1];
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rot);
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;   //La caja de municion se encarga de destruirse al contacto
        if (go.CompareTag("Player"))
        {
            am.ammoDestroyed(position);
            Destroy(gameObject);
        }
    }

}
