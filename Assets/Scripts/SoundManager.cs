using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

enum BGMMusicTheme
{
    Gameplay = 0,
    MainMenu = 1,
}

public enum SfxEffect
{
    PlayerAttack1 = 0,
    PlayerAttack2 = 1,
    PlayerAttack3 = 2,
    Bankai = 3,
    Burning = 4,
    Checkpoint = 5,
    BlackHole = 6,
    Click = 7,
    Clock = 8,
    ClockTick1 = 9,
    ClockTick2 = 10,
    DeathScreen = 11,
    EvilVoice = 12,
    FireMagic = 13,
    PlayerFootstep = 14,
    GirlSigh2 = 15,
    GrandfatherClock = 16,
    PlayerJump = 17,
    ItemPickup = 18,
    MonsterBreath = 19,
    MonsterGrowl1 = 20,
    MonsterGrowl2 = 21,
    OpenChest = 22,
    QuickTimeEvent = 23,
    SkeletonDie = 24,
    Spell1 = 25,
    Spell2 = 26,
    SwordThrow1 = 27,
    SwordThrow2 = 28,
    ThunderStrike = 29,
    Wind = 30,
    WomenSigh1 = 31,
    WomenSigh2 = 32,
    WomenSigh3 = 33,
    WomenStruggle1 = 34,
    WomenStruggle2 = 35, 
    SkeletonAttack1 = 36,
    Heal = 37,
    Jump = 38,
    Hit = 39,
    Equip = 40,
    Unequip = 41,
    Crystal = 42,
    Dash = 43,
    Explosion1 = 44,
    Explosion2 = 45,
    Explosion3 = 46,
    Landing = 47,
    Hit2 = 48,
    FailCraft = 49,
    SuccessCraft = 50,
    FailSkillLearn = 51,
    SuccessSkillLearn = 52,
    CraftFail = 53,
    SkillFail = 54,
    SlimeBite = 55,
    SlimeDie = 56,
    SlimeSpawn = 57,
    SlimeDieMid = 58,
    SlimeDieSmall = 59,
    SlimeBiteMid = 60,
    SlimeBiteSmall = 61,
    ArcherDie = 62,
    ArcherJump = 63,
    ArcherBowLoading = 64,
    ArcherBowRelease = 65,
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
    
    private bool isMuted = true;
    
    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                StatBGM((int)BGMMusicTheme.MainMenu);
                break;
            case "Gameplay":
                StatBGM((int)BGMMusicTheme.Gameplay);
                break;
        }
        
        Invoke(nameof(Unmute), 1f);
    }
    
    public AudioSource GetSFX(int index)
    {
        return sfx[index];
    }
    
    public void PlaySFX(SfxEffect fxIndex, Transform source = null)
    {
        PlaySFX(fxIndex, source, 1f);
    }
    
    public void PlaySFX(SfxEffect fxIndex, Transform source, float pitch)
    {
        if (isMuted)
        {
            return;
        }
        
        if (source != null)
        {
            float distance = Vector3.Distance(source.position, PlayerManager.instance.player.transform.position);
            if (distance > maxDistanceSFX)
            {
                return;
            }
        }

        sfx[(int)fxIndex].pitch = Random.Range(pitch - 0.1f, pitch + 0.1f);
        sfx[(int)fxIndex].Play();
    }

    public void StopSFX(int fxIndex)
    {
        sfx[fxIndex].Stop();
        sfx[fxIndex].loop = false;
    }

    public void StatBGM(int bgIndex)
    {
        foreach (var audioSource in bgm)
        {
            audioSource.Stop();
        }
        bgm[bgIndex].Play();
    }
    
    public Coroutine StopEffectSlowly(int fxIndex)
    {
        return gameObject.IsDestroyed() ? null : StartCoroutine(StopSFXSlowly(fxIndex));
    }
    
    private IEnumerator StopSFXSlowly(int fxIndex)
    {
        var defaultVolume = sfx[fxIndex].volume;
        while (sfx[fxIndex].volume > 0)
        {
            sfx[fxIndex].volume -= Time.deltaTime;
            yield return null;
        }
        sfx[fxIndex].Stop();
        sfx[fxIndex].volume = defaultVolume;
    }
    
    public void StopBGM(int bgIndex)
    {
        bgm[bgIndex].Stop();
    }
    
    private void Unmute()
    {
        isMuted = false;
    }
}
