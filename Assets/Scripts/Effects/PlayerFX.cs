using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Dust Effect")]
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Image FX")]
    [SerializeField] public float imageFrequency;
    [SerializeField] private float disappearSpeed;
    [SerializeField] private GameObject imageFXPrefab;
    
    [Header("Screen Shake")]
    [SerializeField] private float shakeAmplitude;
    public Vector2 swordCatchShake;
    public Vector2 strongHitShake;
    private CinemachineImpulseSource impulseSource;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        impulseSource = FindObjectOfType<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
    public void ShakeScreen(Vector2 direction)
    {
        var impulseVector =
            new Vector3(direction.x * PlayerManager.instance.player.faceDir, direction.y * shakeAmplitude, 0) *
            shakeAmplitude;
        impulseSource.GenerateImpulseWithVelocity(impulseVector);
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
