using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject continueButton;
    [SerializeField] private FadeCanvas fadeCanvas;
    
    [SerializeField] private float mainMenuFadeDuration = 3;
    [SerializeField] private float gameplayFadeDuration = 3;
    
    void Start()
    {
        fadeCanvas.OnFadeInComplete += LoadGameplayScene;
        
        if (!SaveManager.instance.HasSaveData())
            continueButton.SetActive(false);
        
        fadeCanvas.FadeOut(mainMenuFadeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGage()
    {
        SaveManager.instance.DeleteSaveData();
        
        SwitchToGameplay();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadGame()
    {
        SwitchToGameplay();
    }
    
    private void SwitchToGameplay()
    {
        fadeCanvas.FadeIn(gameplayFadeDuration);
    }
    
    private void LoadGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
