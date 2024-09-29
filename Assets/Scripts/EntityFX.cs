using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    
    [SerializeField] private ParticleSystem onHitParticles;
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Image FX")]
    [SerializeField] public float imageFrequency;
    [SerializeField] private float disappearSpeed;
    [SerializeField] private GameObject imageFXPrefab;
    
    [Header("Screen Shake")]
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeAmplitude;
    public Vector2 swordCatchShake;
    public Vector2 strongHitShake;
    
    private Entity entity;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMaterial = sr.material;
        originalColor = sr.color;
        entity = GetComponentInParent<Entity>();
        impulseSource = FindObjectOfType<CinemachineImpulseSource>();
    }
    
    void Update()
    {
        
    }
    
    public void ShakeScreen(Vector2 direction)
    {
        var impulseVector =
            new Vector3(direction.x * PlayerManager.instance.player.faceDir, direction.y * shakeAmplitude, 0) *
            shakeAmplitude;
        impulseSource.GenerateImpulseWithVelocity(impulseVector);
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
    
    public void DustEffect() 
    {
        dustParticles.Play();
    }

    public void ImageFX(Sprite _sprite)
    {
        GameObject imageFX = Instantiate(imageFXPrefab, entity.transform.position, entity.transform.rotation);
        imageFX.GetComponent<ImageFXController>().Setup(disappearSpeed, _sprite);
    }
}
