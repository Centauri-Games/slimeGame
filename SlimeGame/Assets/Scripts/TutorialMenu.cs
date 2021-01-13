using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    public Image tutorial;
    public Sprite spanishPC, englishPC, spanishMobile, englishMobile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MobileChecker.isMobile())
        {
            if (PlayerPrefs.GetInt("language") == 1)
            {
                tutorial.sprite = spanishMobile;
            }
            else
            {
                tutorial.sprite = englishMobile;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("language") == 1)
            {
                tutorial.sprite = spanishPC;
            }
            else
            {
                tutorial.sprite = englishPC;
            }
        }
    }
}
