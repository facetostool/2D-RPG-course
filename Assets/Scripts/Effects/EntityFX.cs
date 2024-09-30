using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Entity entity;

    [Header("Hit Flash")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashTime;
    private Material originalMaterial;
    private Color originalColor;
    [SerializeField] private ParticleSystem onHitParticles;
    
    [Header("Elemental Effects")]
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;
    
    [SerializeField] private ParticleSystem burningParticles;
    [SerializeField] private ParticleSystem chillParticles;
    [SerializeField] private ParticleSystem shockParticles;
    
    [Header("Popup Text")]
    [SerializeField] private GameObject popupTextPrefab;
    [SerializeField] public Color defaultPopupTextColor;
    [SerializeField] public Color critPopupTextColor;
    [SerializeField] public Color fireDmgTextColor;
    [SerializeField] public Color iceDmgTextColor;
    [SerializeField] public Color shockDmgTextColor;
    [SerializeField] public float defaultPopupTextSize;
    [SerializeField] public float bigPopupTextSize;
    [SerializeField] public float smallPopupTextSize;
    
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        originalColor = sr.color;
        entity = GetComponentInParent<Entity>();
    }
    
    protected virtual void Update()
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
    
    public void OnHit()
    {
        onHitParticles.Play();
    }
    
    public void PopupTextFX(string text, Color color, float size)
    {
        // generate random position offset
        Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), 1 + Random.Range(-0.3f, 0.3f), 0);
        GameObject popupText = Instantiate(popupTextPrefab, entity.transform.position + offset, Quaternion.identity);
        popupText.GetComponent<PopupTextFXController>().Setup(text, color, size);
    }
}
