using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (PlayerPrefs.GetInt("language") == 1)
        {
            
        }
        else
        {
            
        }*/
    }


    public void goBackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
