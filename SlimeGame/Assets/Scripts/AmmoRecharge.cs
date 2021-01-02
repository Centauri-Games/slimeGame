using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoRecharge : MonoBehaviour
{
    Vector3 rot = new Vector3(0f, 1f, 0.0f);

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
            Destroy(gameObject);
        }
    }

}
