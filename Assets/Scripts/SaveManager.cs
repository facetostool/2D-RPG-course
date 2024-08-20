using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region singleton

    public static SaveManager instance;
    
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
    
    private FileDataHandler fileDataHandler;
    private GameData gameData;
    private List<ISaveManager> saveManagers;

    [SerializeField] private string dataFileName;
    [SerializeField] private bool needToEncryptData = false;
    
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
        
        if (gameData == null)
        {
            Debug.Log("No save data found");
            NewGame();
        }
        
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        
        Debug.Log("Game loaded");
        Debug.Log("Currency: " + gameData.currency);
    }
    
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        
        fileDataHandler.Save(gameData);
        
        Debug.Log("Game saved");
        Debug.Log("Currency: " + gameData.currency);
    }
    
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        saveManagers = GetListOfSaveManagers();
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, dataFileName, needToEncryptData);
        
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private List<ISaveManager> GetListOfSaveManagers()
    {
        IEnumerable<ISaveManager> managers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(managers);
    }
    
    [ContextMenu("Delete Saved Data")]
    public void DeleteSaveData()
    {
        var fileDataHandler = new FileDataHandler(Application.persistentDataPath, dataFileName, needToEncryptData);
        fileDataHandler.Delete();
        Debug.Log("Saved data deleted");
    }
    
    public bool HasSaveData()
    {
        return fileDataHandler.HasData();
    }
}
