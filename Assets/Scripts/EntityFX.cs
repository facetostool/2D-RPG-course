using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashTime;
    private Material originalMaterial;
    private Color originalColor;
    
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;
    
    [SerializeField] private ParticleSystem burningParticles;
    [SerializeField] private ParticleSystem chillParticles;
    [SerializeField] private ParticleSystem shockParticles;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        originalColor = sr.color;
    }
    
    void Update()
    {
        
    }
    
    public IEnumerator Flash()
    {
        sr.material = flashMaterial;
        Color currentColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashTime);
        sr.material = originalMaterial;
        sr.color = currentColor;
    }

    public void StunnedEffect()
    {
        sr.color = sr.color != Color.white ? Color.white : Color.red;
    }

    public void StopEffect()
    {
        sr.color = originalColor;
        CancelInvoke();
    }
    
    public void IgniteEffectFor(float time)
    {
        burningParticles.Play();
        Invoke(nameof(BurningParticlesStop), time);
        
        InvokeRepeating(nameof(IgniteEffect), 0, 0.3f);
        Invoke(nameof(StopEffect), time);
    }
    
    public void ChillEffectFor(float time)
    {
        chillParticles.Play();
        Invoke(nameof(ChillingParticlesStop), time);
        
        InvokeRepeating(nameof(ChillEffect), 0, 0.3f);
        Invoke(nameof(StopEffect), time);
    }
    
    public void ShockEffectFor(float time)
    {
        shockParticles.Play();
        Invoke(nameof(ShockingParticlesStop), time);
        
        InvokeRepeating(nameof(ShockEffect), 0, 0.3f);
        Invoke(nameof(StopEffect), time);
    }

    void IgniteEffect()
    {
        sr.color = sr.color != igniteColors[0] ? igniteColors[0] : igniteColors[1];
    }
    
    void ChillEffect()
    {
        sr.color = sr.color != chillColors[0] ? chillColors[0] : chillColors[1];
    }
    
    void ShockEffect()
    {
        sr.color = sr.color != shockColors[0] ? shockColors[0] : shockColors[1];
    }
    
    public void MakeTransparent()
    {
        Color tmp = sr.color;
        tmp.a = 0f;
        sr.color = tmp;
    }
    
    private void BurningParticlesStop()
    {
        burningParticles.Stop();
    }
    
    private void ChillingParticlesStop()
    {
        chillParticles.Stop();
    }
    
    private void ShockingParticlesStop()
    {
        shockParticles.Stop();
    }
}
