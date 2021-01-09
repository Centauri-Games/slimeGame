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

    public void goToMainMenuScene(){
        SceneManager.LoadScene("MainMenu");
    }
    public  void slimeCursorAdd(int valueToAdd){
         slimeCursor +=  valueToAdd;
         if (slimeCursor < 0){
             slimeCursor = 0;
         } else if(slimeCursor > slimeList.Length){
             slimeCursor = slimeList.Length;
         }
         PlayerPrefs.GetInt("slimeSkin",slimeCursor);
     }

     public void waterGunCursorAdd(int valueToAdd){
         waterGunCursor +=  valueToAdd;
         if (waterGunCursor < 0){
             waterGunCursor = 0;
         } else if(waterGunCursor > waterGunList.Length){
             waterGunCursor = waterGunList.Length;
         }
         PlayerPrefs.GetInt("waterGunSkin",waterGunCursor);
     }

    public void waterGrenadeCursorAdd(int valueToAdd){
         waterGrenadeCursor +=  valueToAdd;
         if (waterGrenadeCursor < 0){
             waterGrenadeCursor = 0;
         } else if(waterGrenadeCursor > waterGrenadeList.Length){
             waterGrenadeCursor = waterGrenadeList.Length;
         }
          PlayerPrefs.GetInt("waterGrenadeSkin",waterGrenadeCursor);
     }

     public void plungerCursorAdd(int valueToAdd){
         plungerCursor +=  valueToAdd;
         if (plungerCursor < 0){
             plungerCursor = 0;
         } else if(plungerCursor > plungerList.Length){
             plungerCursor = plungerList.Length;
         }
         PlayerPrefs.SetInt("plungerSkin",plungerCursor);
     }


    void Awake(){
        slimeCursor = PlayerPrefs.GetInt("slimeSkin",0);
        waterGunCursor = PlayerPrefs.GetInt("waterGunSkin",0);
        waterGrenadeCursor = PlayerPrefs.GetInt("waterGrenadeSkin",0);
        plungerCursor = PlayerPrefs.GetInt("plungerSkin",0);
    }


    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
