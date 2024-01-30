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
    
    public IEnumerator Flash()
    {
        sr.material = flashMaterial;
        yield return new WaitForSeconds(flashTime);
        sr.material = originalMaterial;
    }
    
    void Update()
    {
        
    }
}
