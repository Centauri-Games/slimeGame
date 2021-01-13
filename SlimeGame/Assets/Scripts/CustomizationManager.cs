using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    GameObject actualSlime;

    GameObject actualWaterGun;

    GameObject actualWaterGrenade;

    GameObject actualPlunger;

    public void goToMainMenuScene(){
        SceneManager.LoadScene("MainMenu");
    }
    public  void slimeCursorAdd(int valueToAdd){
        slimeList[slimeCursor].SetActive(false);
         slimeCursor +=  valueToAdd;
         if (slimeCursor < 0){
             slimeCursor = 0;
         } else if(slimeCursor > slimeList.Length - 1){
             slimeCursor = slimeList.Length - 1;
         }
         actualSlime = slimeList[slimeCursor];
        actualSlime.SetActive(true);
         PlayerPrefs.GetInt("slimeSkin",slimeCursor);
     }

     public void waterGunCursorAdd(int valueToAdd){
         waterGunList[waterGunCursor].SetActive(false);
         waterGunCursor +=  valueToAdd;
         if (waterGunCursor < 0){
             waterGunCursor = 0;
         } else if(waterGunCursor > waterGunList.Length - 1){
             waterGunCursor = waterGunList.Length - 1;
         }
         actualWaterGun =  waterGunList[waterGunCursor]; 
           actualWaterGun.SetActive(true);
         PlayerPrefs.SetInt("waterGunSkin",waterGunCursor);
     }

    public void waterGrenadeCursorAdd(int valueToAdd){
        waterGrenadeList[waterGrenadeCursor].SetActive(false);
         waterGrenadeCursor +=  valueToAdd;
         if (waterGrenadeCursor < 0){
             waterGrenadeCursor = 0;
         } else if(waterGrenadeCursor > waterGrenadeList.Length -1){
             waterGrenadeCursor = waterGrenadeList.Length -1;
         }
          actualWaterGrenade = waterGrenadeList[waterGrenadeCursor];
            actualWaterGrenade.SetActive(true);
          PlayerPrefs.SetInt("waterGrenadeSkin",waterGrenadeCursor);
     }

     public void plungerCursorAdd(int valueToAdd){
         plungerList[plungerCursor].SetActive(false);
         plungerCursor +=  valueToAdd;
         if (plungerCursor < 0){
             plungerCursor = 0;
         } else if(plungerCursor > plungerList.Length -1){
             plungerCursor = plungerList.Length - 1;
         }
         actualPlunger = plungerList[plungerCursor];
            actualPlunger.SetActive(true);
         PlayerPrefs.SetInt("plungerSkin",plungerCursor);
     }


    void Awake(){
        slimeCursor = PlayerPrefs.GetInt("slimeSkin",0);
            for(int i = 0; i < slimeList.Length; i++){
                slimeList[i].SetActive(false);
            }
            actualSlime = slimeList[slimeCursor];
             actualSlime.SetActive(true);


        waterGunCursor = PlayerPrefs.GetInt("waterGunSkin",0);
        for(int i = 0; i < waterGunList.Length; i++){
                waterGunList[i].SetActive(false);
            }
            actualWaterGun =  waterGunList[waterGunCursor]; 
           actualWaterGun.SetActive(true);


        waterGrenadeCursor = PlayerPrefs.GetInt("waterGrenadeSkin",0);
        for(int i = 0; i < waterGrenadeList.Length; i++){
                waterGrenadeList[i].SetActive(false);
            }
            actualWaterGrenade = waterGrenadeList[waterGrenadeCursor];
            actualWaterGrenade.SetActive(true);


        plungerCursor = PlayerPrefs.GetInt("plungerSkin",0);
        for(int i = 0; i < plungerList.Length; i++){
                plungerList[i].SetActive(false);
            }
            actualPlunger = plungerList[plungerCursor];
            actualPlunger.SetActive(true);
    }


    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
