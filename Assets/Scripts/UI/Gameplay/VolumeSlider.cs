using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, ISaveManager
{
    
    [SerializeField] private string mixerParameter;
    [SerializeField] private AudioMixer audioMixer;
    
    void Update()
    {
        
    }
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(mixerParameter, Mathf.Log10(volume) * 25);
    }

    public void SaveData(ref GameData data)
    {
        data.volumeLevels[mixerParameter] = GetComponent<Slider>().value;
    }

    public void LoadData(GameData data)
    {
        var slider = GetComponent<Slider>();
        
        if (!data.volumeLevels.ContainsKey(mixerParameter))
        {
            slider.value = 1; 
            return;
        }
        
        slider.value = data.volumeLevels[mixerParameter];
    }
}
