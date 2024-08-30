using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectArea : MonoBehaviour
{
    // Start is called before the first frame update
    private Coroutine stopWindCoroutine;
    private float defaultValue;
    private AudioSource windAudioSource;
    
    void Start()
    {
        windAudioSource = SoundManager.instance.GetSFX((int)SfxEffect.Wind);
        defaultValue = windAudioSource.volume;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider.OverlapPoint(PlayerManager.instance.player.transform.position)) {
            SoundManager.instance.PlaySFX((int)SfxEffect.Wind);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!windAudioSource.isPlaying)
            windAudioSource.Play();
            
        if (stopWindCoroutine == null) return;
        StopCoroutine(stopWindCoroutine);
        windAudioSource.volume = defaultValue;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.GetComponent<Player>()) return;
        Debug.Log("Exit the trigger");
        stopWindCoroutine = SoundManager.instance.StopEffectSlowly((int)SfxEffect.Wind);
    }
}
