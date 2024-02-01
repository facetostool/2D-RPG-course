using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFlashFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashTime;
    private Material originalMaterial;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
    }
    
    void Update()
    {
        
    }
    
    public IEnumerator Flash()
    {
        sr.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        sr.material = originalMaterial;
    }

    public void StunnedEffect()
    {
        if (sr.color == Color.white)
        {
            sr.color = Color.red; 
            return;
        }
        
        if (sr.color == Color.red) {
            sr.color = Color.white;
            return;
        }
    }

    public void StopStunnedEffect()
    {
        sr.color = Color.white;
        CancelInvoke();
    }
    
}
