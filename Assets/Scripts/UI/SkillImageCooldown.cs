using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillImageCooldown : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private float cooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (background.fillAmount > 0)
        {
            background.fillAmount -= 1 / cooldown * Time.deltaTime;
        }  
    }
    
    public void StartCooldown(float _cooldown)
    {
        background.fillAmount = 1;
        cooldown = _cooldown;
    }
}
