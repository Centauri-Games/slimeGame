using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToMatchmakingScene(){
        SceneManager.LoadScene("Menu");
    }

    public void goBackToMenu(){
        SceneManager.LoadScene("GameMenu");
    }
    
}
