using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("Fade canvas")]
    [SerializeField] public FadeCanvas fadeCanvas;
    [SerializeField] public float fadeDuration = 0.5f;
    
    [Header("Fade died canvas")]
    [SerializeField] public FadeDieCanvas fadeDiedCanvas;
    [SerializeField] public float fadeDiedDuration = 0.5f;
    
    [Header("Tooltips")]
    [SerializeField] public ItemTooltip itemTooltip;
    [SerializeField] public StatTooltip statTooltip;
    [SerializeField] public SkillTooltip skillTooltip;
    
    [Header("Tabs")]
    [SerializeField] public GameObject inventoryTab;
    [SerializeField] public GameObject craftTab;
    [SerializeField] public GameObject skillTreeTab;
    [SerializeField] public GameObject optionsTab;
    [SerializeField] public GameObject inGameUI;
    
    public CraftItemWindow craftItemWindow;
    
    // Start is called before the first frame update
    void Start()
    {   
        craftItemWindow = GetComponentInChildren<CraftItemWindow>();
        
        HideTooltips();
        SwitchToTab(inGameUI);
        
        fadeCanvas.FadeOut(fadeDuration);
        
        PlayerManager.instance.player.onDie += OnDieTrigger;
    }

    private void HideTooltips()
    {
        itemTooltip.HideTooltip();
        statTooltip.HideTooltip();
        skillTooltip.HideTooltip();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchToTabByKey(inventoryTab);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchToTabByKey(craftTab);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToTabByKey(skillTreeTab);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToTabByKey(optionsTab);
        }
    }
    
    public void SwitchToTab(GameObject tab)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        if (tab != null)
            tab.SetActive(true);
        
        SoundManager.instance.PlaySFX(SfxEffect.Click);
        HideTooltips();
        ShowInGameUI();
    }

    private void SwitchToTabByKey(GameObject tab)
    {
        if (tab != null && tab.activeSelf)
        {
            tab.SetActive(false);
            ShowInGameUI();
            return;
        }
        SwitchToTab(tab);
    }
    
    private void ShowInGameUI()
    {
        // if all tabs are closed, show InGameUI
        if (transform.Cast<Transform>().Any(child => child.gameObject.activeSelf))
            return;

        inGameUI.SetActive(true);
    }
    
    public void OnDieTrigger()
    {
        Invoke(nameof(ShowDiedCanvas), 1);
    }
    
    private void ShowDiedCanvas()
    {
        fadeDiedCanvas.FadeIn(fadeDiedDuration);
        SoundManager.instance.PlaySFX(SfxEffect.DeathScreen);
    }
}
