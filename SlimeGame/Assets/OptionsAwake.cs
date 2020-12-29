using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionsAwake : MonoBehaviour
{

    [SerializeField] Slider sliderVolume;
    [SerializeField] Slider sliderSensitivity;

    [SerializeField] Button español;
    [SerializeField] Button english;
    void Awake(){
        sliderVolume.value = PlayerPrefs.GetFloat("volume",1);
        sliderVolume.onValueChanged.AddListener(delegate {AudioListener.volume = sliderVolume.value;
                                                    PlayerPrefs.SetFloat("volume", sliderVolume.value);
                                                    Debug.Log(sliderVolume.value); });

        sliderSensitivity.value = PlayerPrefs.GetFloat("sensitivity",1);
        sliderSensitivity.onValueChanged.AddListener(delegate {
                                                    PlayerPrefs.SetFloat("sensitivity", sliderSensitivity.value);
                                                    Debug.Log(sliderSensitivity.value); });

        español.onClick.AddListener(delegate {PlayerPrefs.SetInt("language",1);});
        english.onClick.AddListener(delegate {PlayerPrefs.SetInt("language",0);});
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        
    }
}
