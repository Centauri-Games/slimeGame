using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOptions : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject options;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)){
            if(!options.active){
                options.SetActive(true);   
            }else {
                options.SetActive(false);   
            }
            
        }
    }
}
