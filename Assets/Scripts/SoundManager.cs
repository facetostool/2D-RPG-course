using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum BGMMusicTheme
{
    Gameplay = 0,
    MainMenu = 1,
}

enum SfxEffect
{
    PlayerAttack = 2,
    PlayerDie = 35, 
    PlayerFootstep = 14,
    ItemPickup = 18,
    PlayerJump = 17,
    SkeletonDie = 24,
    Checkpoint = 5,
}
public class SoundManager : MonoBehaviour
{
    #region singleton

    public static SoundManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    
    [SerializeField] private float maxDistanceSFX = 10f;
    
    private void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                SoundManager.instance.StatBGM((int)BGMMusicTheme.MainMenu);
                break;
            case "Gameplay":
                SoundManager.instance.StatBGM((int)BGMMusicTheme.Gameplay);
                break;
        }
    }
    
    public void PlaySFX(int fxIndex, Transform source = null)
    {
        if (source != null)
        {
            float distance = Vector3.Distance(source.position, PlayerManager.instance.player.transform.position);
            if (distance > maxDistanceSFX)
            {
                return;
            }
        }

        sfx[fxIndex].pitch = Random.Range(0.9f, 1.1f);
        sfx[fxIndex].Play();
    }

    public void StopSFX(int fxIndex)
    {
        sfx[fxIndex].Stop();
    }

    public void StatBGM(int bgIndex)
    {
        foreach (var audioSource in bgm)
        {
            audioSource.Stop();
        }
        bgm[bgIndex].Play();
    }
    
    public void StopBGM(int bgIndex)
    {
        bgm[bgIndex].Stop();
    }
}
