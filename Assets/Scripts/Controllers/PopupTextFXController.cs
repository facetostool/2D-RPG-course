using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextFXController : MonoBehaviour
{
    [SerializeField] private float slowSpeed;
    [SerializeField] private float fastSpeed;
    [SerializeField] private float timeBeforeSlideFast;
    [SerializeField] private float fadeSpeed;

    private float timer;
    private TextMeshPro text;
    
    public void Setup(string textValue, Color color, float size)
    {
        text = GetComponent<TextMeshPro>();
        text.text = textValue;
        text.color = color;
        text.fontSize = size;
    }
    
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer < timeBeforeSlideFast)
        {
            transform.position += Vector3.up * slowSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.up * fastSpeed * Time.deltaTime;
            if (text.alpha > 0)
            {
                text.alpha -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
