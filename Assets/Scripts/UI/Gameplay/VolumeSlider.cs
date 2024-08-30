using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, ISaveManager
{
    
    [SerializeField] private string mixerParameter;
    [SerializeField] private AudioMixer audioMixer;
    
    private Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    
    void Update()
    {
        
    }
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(mixerParameter, Mathf.Log10(volume) * 25);
    }

    public void SaveData(ref GameData data)
    {
        data.volumeLevels[mixerParameter] = slider.value;
    }

    public void LoadData(GameData data)
    {
        if (!data.volumeLevels.ContainsKey(mixerParameter))
        {
            slider.value = 0; 
            return;
        }
        
        slider.value = data.volumeLevels[mixerParameter];
    }
}
