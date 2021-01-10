using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    void Awake(){
        PlayerController playercontroller = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        GetComponent<Slider>().onValueChanged.AddListener(delegate{
           playercontroller.sensitivityX = 15*PlayerPrefs.GetFloat("sensitivity",1);
           
           playercontroller.sensitivityY = 15*PlayerPrefs.GetFloat("sensitivity",1);
           Debug.Log(playercontroller.sensitivityX + playercontroller.sensitivityY);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
