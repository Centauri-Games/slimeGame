using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject[] slimeList;
    [SerializeField] GameObject[] waterGunList;

    [SerializeField] GameObject[] waterGrenadeList;

    [SerializeField] GameObject[] plungerList;

    int slimeCursor;
    int waterGunCursor;
    int waterGrenadeCursor;
    int plungerCursor;


     void slimeCursorAdd(int valueToAdd){
         slimeCursor +=  valueToAdd;
         if (slimeCursor < 0){
             slimeCursor = 0;
         } else if(slimeCursor > slimeList.Length){
             slimeCursor = slimeList.Length;
         }
     }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
