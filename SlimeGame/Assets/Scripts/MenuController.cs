﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume",1);
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
