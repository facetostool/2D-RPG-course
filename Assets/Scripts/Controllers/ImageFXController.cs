using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFXController : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer sr;
    private float disappearSpeed;
    
    public void Setup(float _disappearSpeed, Sprite _sprite)
    {
        sr.sprite = _sprite;
        disappearSpeed = _disappearSpeed;
    }
    
    void Update()
    {
        if (sr.color.a > 0)
        {
            Color currentColor = sr.color;
            currentColor.a -= disappearSpeed * Time.deltaTime;
            sr.color = currentColor;
        }
        
        if (sr.color.a <= 0)
            Destroy(gameObject);
    }
}
