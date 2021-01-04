using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOptions : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject options;
    [SerializeField] GameObject ingame;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(!options.active){
                options.SetActive(true);
                ingame.SetActive(false);
            }else {
                options.SetActive(false);
                ingame.SetActive(true);
            }
            
        }
    }
}
