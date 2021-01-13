using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsLanguage : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("language") == 1)
        {
            text.text = "Jugadores           Puntuación";
        }
        else
        {
            text.text = "Players           Score";
        }
    }
}
